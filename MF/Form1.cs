using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using MediaFoundation;
using MediaFoundation.Misc;
using MediaFoundation.ReadWrite;

namespace MF
{

    public class MediaFoundationCamera
    {
        private IMFSourceReader _reader;
        private Thread _captureThread;
        private bool _running = false;
        private Bitmap _currentFrame;
        private PictureBox _targetBox;

        public MediaFoundationCamera()
        {
            MFExtern.MFStartup(0x00020070, MFStartup.Full);
        }

        public List<string> ListCameraNames()
        {
            List<string> names = new List<string>();
            IMFAttributes attr;
            MFExtern.MFCreateAttributes(out attr, 1);
            attr.SetGUID(MFAttributesClsid.MF_DEVSOURCE_ATTRIBUTE_SOURCE_TYPE,
                         MFAttributesClsid.MF_DEVSOURCE_ATTRIBUTE_SOURCE_TYPE_VIDCAP_GUID);

            IMFActivate[] devices;
            int count;
            MFExtern.MFEnumDeviceSources(attr, out devices, out count);

            for (int i = 0; i < count; i++)
            {
                devices[i].GetString(MFAttributesClsid.MF_DEVSOURCE_ATTRIBUTE_FRIENDLY_NAME, out string name, 128);
                names.Add(name);
            }

            return names;
        }

        public bool StartPreview(string cameraName, PictureBox pictureBox)
        {
            _targetBox = pictureBox;

            IMFActivate camera = FindCameraByName(cameraName);
            if (camera == null)
                return false;

            camera.ActivateObject(typeof(IMFMediaSource).GUID, out object sourceObj);
            IMFMediaSource source = sourceObj as IMFMediaSource;

            IMFAttributes attr;
            MFExtern.MFCreateAttributes(out attr, 1);
            attr.SetUINT32(MFAttributesClsid.MF_READWRITE_DISABLE_CONVERTERS, 0);

            MFExtern.MFCreateSourceReaderFromMediaSource(source, attr, out _reader);

            _running = true;
            _captureThread = new Thread(CaptureLoop)
            {
                IsBackground = true
            };
            _captureThread.Start();
            return true;
        }

        public void StopPreview()
        {
            _running = false;
            _captureThread?.Join();

            if (_reader != null)
            {
                Marshal.ReleaseComObject(_reader);
                _reader = null;
            }
            _currentFrame?.Dispose();
        }

        public bool CaptureImage(string savePath)
        {
            if (_currentFrame == null) return false;
            lock (_currentFrame)
            {
                _currentFrame.Save(savePath);
            }
            return true;
        }

        private IMFActivate FindCameraByName(string name)
        {
            IMFAttributes attr;
            MFExtern.MFCreateAttributes(out attr, 1);
            attr.SetGUID(MFAttributesClsid.MF_DEVSOURCE_ATTRIBUTE_SOURCE_TYPE,
                         MFAttributesClsid.MF_DEVSOURCE_ATTRIBUTE_SOURCE_TYPE_VIDCAP_GUID);

            IMFActivate[] devices;
            int count;
            MFExtern.MFEnumDeviceSources(attr, out devices, out count);

            foreach (var device in devices)
            {
                device.GetString(MFAttributesClsid.MF_DEVSOURCE_ATTRIBUTE_FRIENDLY_NAME, out string devName, 128);
                if (devName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return device;
            }
            return null;
        }

        private void CaptureLoop()
        {
            while (_running)
            {
                _reader.ReadSample(MF_SOURCE_READER.FirstVideoStream, 0,
                    out int _, out int _, out long _, out IMFSample sample);

                if (sample != null)
                {
                    sample.ConvertToContiguousBuffer(out IMFMediaBuffer buffer);
                    buffer.Lock(out IntPtr ptr, out int maxLen, out int currLen);

                    Bitmap bmp = new Bitmap(640, 480, 640 * 3,
                        System.Drawing.Imaging.PixelFormat.Format24bppRgb, ptr);

                    lock (this)
                    {
                        _currentFrame?.Dispose();
                        _currentFrame = (Bitmap)bmp.Clone();
                    }

                    _targetBox.Invoke((Action)(() =>
                    {
                        _targetBox.Image?.Dispose();
                        _targetBox.Image = (Bitmap)_currentFrame.Clone();
                    }));

                    bmp.Dispose();
                    buffer.Unlock();
                    Marshal.ReleaseComObject(buffer);
                    Marshal.ReleaseComObject(sample);
                }

                Thread.Sleep(30);
            }
        }

        ~MediaFoundationCamera()
        {
            StopPreview();
            MFExtern.MFShutdown();
        }
    }

}
