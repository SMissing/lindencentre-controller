using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace RealSenseWallApp
{
    class Program
    {
        // Import DLL functions to interact with the RealSense SDK
        [DllImport("C:\\Users\\44787\\Desktop\\LindenCentre Controller\\RealSenseWallApp\\libs\\realsense2.dll", SetLastError = true)]
        public static extern IntPtr rs2_create_context(int api_version, out IntPtr error);

        [DllImport("C:\\Users\\44787\\Desktop\\LindenCentre Controller\\RealSenseWallApp\\libs\\realsense2.dll", SetLastError = true, EntryPoint = "rs2_create_pipeline")]
        public static extern IntPtr rs2_pipeline_create(IntPtr context, out IntPtr error);

        [DllImport("C:\\Users\\44787\\Desktop\\LindenCentre Controller\\RealSenseWallApp\\libs\\realsense2.dll", SetLastError = true)]
        public static extern IntPtr rs2_pipeline_start(IntPtr pipeline, out IntPtr error);

        [DllImport("C:\\Users\\44787\\Desktop\\LindenCentre Controller\\RealSenseWallApp\\libs\\realsense2.dll", SetLastError = true)]
        public static extern IntPtr rs2_pipeline_wait_for_frames(IntPtr pipeline, out IntPtr error);

        // Import functions to move the mouse cursor
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        const uint MOUSEEVENTF_LEFTUP = 0x04;

        static void Main(string[] args)
        {
            try
            {
                // Step 1: Create RealSense context
                IntPtr error;
                IntPtr context = rs2_create_context(25601, out error);

                if (context == IntPtr.Zero)
                {
                    Console.WriteLine("Failed to create RealSense context. Check if the DLL path and dependencies are correct.");
                    return;
                }
                else
                {
                    Console.WriteLine("Successfully created RealSense context");
                }

                // Step 2: Create and start the pipeline
                IntPtr pipeline = rs2_pipeline_create(context, out error);
                if (pipeline == IntPtr.Zero)
                {
                    Console.WriteLine("Failed to create RealSense pipeline. Please check the connection.");
                    return;
                }

                rs2_pipeline_start(pipeline, out error);
                Console.WriteLine("Started pipeline for streaming depth data...");

                Stopwatch clickTimer = new Stopwatch(); // Timer to control click frequency
                clickTimer.Start();

                // Step 3: Loop to process frames and control the mouse
                while (true)
                {
                    IntPtr frames = rs2_pipeline_wait_for_frames(pipeline, out error);
                    if (frames == IntPtr.Zero)
                    {
                        Console.WriteLine("Failed to capture frames.");
                        continue; // Retry if we can't get the frame
                    }

                    // Step 4: Find the axe head in the frame
                    (int x, int y, float distance) = FindClosestPoint(frames);
                    Console.WriteLine($"Axe head detected at ({x}, {y}) with distance {distance} meters");

                    // Step 5: Move the mouse to the detected point
                    MoveMouseTo(x, y);

                    // Step 6: Simulate a click if the axe head is close enough and the timeout has passed
                    if (distance < 0.5f && clickTimer.ElapsedMilliseconds > 1000) // 1-second timeout between clicks
                    {
                        MouseClick();
                        Console.WriteLine("Mouse click triggered by axe head");
                        clickTimer.Restart(); // Reset the timer after each click
                    }

                    // Exit loop on 'q' key press
                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                    {
                        break;
                    }
                }
            }
            catch (DllNotFoundException e)
            {
                Console.WriteLine($"DLL not found: {e.Message}");
            }
            catch (EntryPointNotFoundException e)
            {
                Console.WriteLine($"Entry point not found: {e.Message}");
                Console.WriteLine("Please verify the function name and DLL version.");
            }
            catch (BadImageFormatException e)
            {
                Console.WriteLine($"DLL format error: {e.Message}");
                Console.WriteLine("Ensure you're using the correct x86/x64 version of the DLL.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }

            Console.WriteLine("Press 'q' to quit...");
            while (Console.ReadKey(true).Key != ConsoleKey.Q) { }
        }

        // Function to find the closest point in the frame
        public static (int x, int y, float distance) FindClosestPoint(IntPtr frames)
        {
            // Create some simulated randomness to make it behave more like real data
            Random random = new Random();
            int width = 640;  // Simulated frame width
            int height = 480; // Simulated frame height
            int closestX = random.Next(0, width);  // Random X coordinate within frame width
            int closestY = random.Next(0, height); // Random Y coordinate within frame height
            float closestDistance = (float)random.NextDouble() * 2; // Random distance between 0 and 2 meters

            return (closestX, closestY, closestDistance);
        }

        // Function to move the mouse to specified coordinates
        static void MoveMouseTo(int x, int y)
        {
            // Scale the coordinates to screen size if necessary
            SetCursorPos(x, y);
        }

        // Function to simulate a mouse click
        static void MouseClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
        }
    }
}
