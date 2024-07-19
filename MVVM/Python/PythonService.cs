using System;
using Python.Runtime;
using MVVM.Python;
using System.IO;

namespace MVVM.Python
{
    internal class PythonService
    {
        static internal string ProcessPhoto(string inputImagePath)
        {
            // Initialize the Python runtime
            Runtime.PythonDLL = @"C:\Program Files\Python312\python312.dll";
            PythonEngine.Initialize();

            string tempFilePath = null;

            // Import the Python module
            using (Py.GIL()) // Acquire the GIL (Global Interpreter Lock)
            {
                dynamic sys = Py.Import("sys");
                string assemblyDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                if(assemblyDirectory == null)
                {
                    throw new Exception("Assembly directory is null");
                }
                string scriptDirectory = Path.Combine(assemblyDirectory, "Python");
                sys.path.append(scriptDirectory);

                dynamic example = Py.Import("tresholding");

                // Call the Python function
                dynamic method = example.GetAttr("main");

                tempFilePath = Path.GetTempFileName() + ".jpg";

                method(inputImagePath, tempFilePath);
            }

            // Shutdown the Python runtime
            PythonEngine.Shutdown();
            return tempFilePath;
        }

        static internal string ExportJson(string processedImagePath)
        {
            // Initialize the Python runtime
            Runtime.PythonDLL = @"C:\Program Files\Python312\python312.dll";
            PythonEngine.Initialize();

            string tempFilePath = null;

            // Import the Python module
            using (Py.GIL()) // Acquire the GIL (Global Interpreter Lock)
            {
                dynamic sys = Py.Import("sys");
                string assemblyDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                if (assemblyDirectory == null)
                {
                    throw new Exception("Assembly directory is null");
                }
                string scriptDirectory = Path.Combine(assemblyDirectory, "Python");
                sys.path.append(scriptDirectory);

                dynamic example = Py.Import("jsonexport");

                // Call the Python function
                dynamic method = example.GetAttr("main");

                tempFilePath = Path.GetTempFileName() + ".json";

                method(processedImagePath, tempFilePath);
            }

            // Shutdown the Python runtime
            PythonEngine.Shutdown();
            return tempFilePath;
        }
    }
}
