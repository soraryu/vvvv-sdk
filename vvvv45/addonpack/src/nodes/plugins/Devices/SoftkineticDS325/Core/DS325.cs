using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VVVV.Nodes.DS325;
using System.Threading;
using VVVV.Core.Logging;

namespace VVVV.Nodes.DS325
{
    public delegate void OnFrameEvent();

    /// <summary>
    /// Accesses the DS325. Holds a thread to do background work.
    /// </summary>
    public class DS325Device : IInitialized, IDisposable
    {
        
        Thread thread;
        public bool IsInitialized
        {
            get { return isInitialized; }
        }

        bool isInitialized;

        volatile bool runThread = true;

        /// <summary>
        /// Device mode. Can be any combination of GESTURE, FACE_LANDMARK, FACE_LOCATION, CAPTURE
        /// </summary>
        public PXCUPipelineOT.Mode Mode { get { return mode; } set { mode = value; } }
        volatile PXCUPipelineOT.Mode mode;

        public OnInitEvent OnInit { get; set; }
        public OnDisposeEvent OnDispose { get; set; }
        public OnFrameEvent OnFrame { get; set; }

        /// <summary>
        /// Device thread. Initializes the device and runs Update every few milliseconds.
        /// </summary>
        void UpdateLoop()
        {
            // Initialize PXCUPipeline
            InitDevice();
			
            /*
            while (runThread)
            {
                // is sleep needed? does it do that internally?
                // (of course, sleep is always needed!)
                Thread.Sleep(15);
                Update();
            }
            */
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
        }
        string errorMessage = "";

        public ILogger Logger
        {
            get;
            set;
        }

        public PXCUPipelineOT Pipeline
        {
            get
            {
                if (pp == null)
                    pp = new PXCUPipelineOT();
                return pp;
            }
        }
        private PXCUPipelineOT pp;

        public DS325Device()
        {
            OnInit = delegate { };
            OnDispose = delegate { };
            OnFrame = delegate { };
        }

        /// <summary>
        /// Default Constructor, inits with all tracking modes
        /// </summary>
        public void Init()
        {
            Init(PXCUPipelineOT.Mode.GESTURE | PXCUPipelineOT.Mode.FACE_LANDMARK | PXCUPipelineOT.Mode.FACE_LOCATION);
        }

        /// <summary>
        /// Constructor with certain run modes
        /// </summary>
        public void Init(PXCUPipelineOT.Mode mode)
        {
            Mode = mode;

            runThread = true;
            thread = new Thread(new ThreadStart(UpdateLoop));
            thread.Start();
        }

        void Log(string s)
        {
            errorMessage = s;
            if (Logger != null) Logger.Log(LogType.Debug, s);
        }

        /// <summary>
        /// Initialize the PXCU Pipeline with a certain mode.
        /// </summary>
        public void InitDevice()
        {
            Log("Initializing Camera Device");

            isInitialized = Pipeline.Init(Mode);
            OnInit(isInitialized);

            if (!isInitialized)
            {
                Log("Unable to initialize the PXCUPipeline\n");
            }
            else
            {
                Log("PXCU initialised.");
            }
        }

        /// <summary>
        /// Stop the device thread and close the PXCU Pipeline
        /// </summary>
        public void Dispose()
        {
            // tell Update thread to stop
            runThread = false;
            isInitialized = false;

            // wait for Thread
            if (thread != null)
                thread.Join();

            if (Logger != null)
                Log("Closing PXCU Pipeline");

            Pipeline.Close();
            pp = null;

            OnDispose(true);
        }

        /// <summary>
        /// Aquire the current frame to update all values; release it again
        /// </summary>
        /// <remarks>
        /// Seems PXCUPipeline in itself is thread safe! Great!
        /// </remarks>
        public void Update()
        {
        	if(!isInitialized) return;
            if (!Pipeline.AcquireFrame(true)) return;
            OnFrame();
            Pipeline.ReleaseFrame();
        }

        ~DS325Device()
        {
            Dispose();
        }
    }
}
