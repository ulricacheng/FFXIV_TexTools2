﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseViewModel.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// <summary>
//   Base ViewModel for Demo Applications?
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIV_TexTools2.ViewModel
{
    using System;
    using System.Collections.Generic;

    using HelixToolkit.Wpf.SharpDX;
    using System.IO;

    /// <summary>
    /// Base ViewModel for Demo Applications?
    /// </summary>
    public abstract class BaseViewModel : ObservableObject, IDisposable
    {
        public const string Orthographic = "Orthographic Camera";

        public const string Perspective = "Perspective Camera";

        private string cameraModel;

        private Camera camera;

        private RenderTechnique renderTechnique;

        private string subTitle;

        private string title;

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                SetValue(ref title, value, "Title");
            }
        }

        public string SubTitle
        {
            get
            {
                return subTitle;
            }
            set
            {
                SetValue(ref subTitle, value, "SubTitle");
            }
        }

        public RenderTechnique RenderTechnique
        {
            get
            {
                return renderTechnique;
            }
            set
            {
                SetValue(ref renderTechnique, value, "RenderTechnique");
            }
        }

        public List<string> ShadingModelCollection { get; private set; }

        public List<string> CameraModelCollection { get; private set; }

        public string CameraModel
        {
            get
            {
                return cameraModel;
            }
            set
            {
                if (SetValue(ref cameraModel, value, "CameraModel"))
                {
                    OnCameraModelChanged();
                }
            }
        }

        public Camera Camera
        {
            get
            {
                return camera;
            }

            protected set
            {
                SetValue(ref camera, value, "Camera");
                CameraModel = value is PerspectiveCamera
                                       ? Perspective
                                       : value is OrthographicCamera ? Orthographic : null;
            }
        }

        public IEffectsManager EffectsManager { get; protected set; }

        public IRenderTechniquesManager RenderTechniquesManager { get; protected set; }

        protected OrthographicCamera defaultOrthographicCamera = new OrthographicCamera { Position = new System.Windows.Media.Media3D.Point3D(0, 0, 5), LookDirection = new System.Windows.Media.Media3D.Vector3D(-0, -0, -5), UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0), NearPlaneDistance = 1, FarPlaneDistance = 100 };

        protected PerspectiveCamera defaultPerspectiveCamera = new PerspectiveCamera { Position = new System.Windows.Media.Media3D.Point3D(0, 0, 5), LookDirection = new System.Windows.Media.Media3D.Vector3D(-0, -0, -5), UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0), NearPlaneDistance = 0.5, FarPlaneDistance = 150 };

        public event EventHandler CameraModelChanged;

        protected BaseViewModel()
        {
            // camera models
            CameraModelCollection = new List<string>()
            {
                Orthographic,
                Perspective,
            };

            // on camera changed callback
            CameraModelChanged += (s, e) =>
            {
                if (cameraModel == Orthographic)
                {
                    //if (this.Camera != null)
                    //{
                    //    var newCamera = new OrthographicCamera();
                    //    this.Camera.CopyTo(newCamera);
                    //    newCamera.NearPlaneDistance = znear;
                    //    newCamera.FarPlaneDistance = zfar;
                    //    this.Camera = newCamera;

                    //}
                    //else
                    {
                        Camera = defaultOrthographicCamera;
                    }
                }
                else if (cameraModel == Perspective)
                {
                    //if (this.Camera != null)
                    //{
                    //    var newCamera = new PerspectiveCamera();
                    //    this.Camera.CopyTo(newCamera);
                    //    newCamera.NearPlaneDistance = znear;
                    //    newCamera.FarPlaneDistance = zfar;
                    //    this.Camera = newCamera;
                    //}
                    //else
                    {
                        Camera = defaultPerspectiveCamera;
                    }
                }
                else
                {
                    throw new HelixToolkitException("Camera Model Error.");
                }
            };

            // default camera model
            CameraModel = Perspective;

            Title = "Demo (HelixToolkitDX)";
            SubTitle = "Default Base View Model";
        }

        protected virtual void OnCameraModelChanged()
        {
            var eh = CameraModelChanged;
            if (eh != null)
            {
                eh(this, new EventArgs());
            }
        }

        public static MemoryStream LoadFileToMemory(string filePath)
        {
            using (var file = new FileStream(filePath, FileMode.Open))
            {
                var memory = new MemoryStream();
                file.CopyTo(memory);
                return memory;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                if (EffectsManager != null)
                {
                    var effectManager = EffectsManager as IDisposable;
                    Disposer.RemoveAndDispose(ref effectManager);
                }
                disposedValue = true;
                GC.SuppressFinalize(this);
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~BaseViewModel()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}