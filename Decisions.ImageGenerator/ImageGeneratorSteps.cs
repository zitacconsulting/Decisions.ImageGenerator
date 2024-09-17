using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using DecisionsFramework.Design.Flow;

namespace Zitac.ImageGenerator.Steps;

[AutoRegisterMethodsOnClass(true, "Integration", "Image Generator")]
public class ImageGeneratorSteps
{

    public static byte[] GenerateObfuscatedPasswordImage(string password, int width = 300, int height = 100)
    {
        // Create a bitmap image with the specified width and height
        using (Bitmap bitmap = new Bitmap(width, height))
        {
            // Create a graphics object from the bitmap
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // Set background color
                graphics.Clear(Color.White);

                // Add random noise (dots) to the background
                Random rand = new Random();
                for (int i = 0; i < 100; i++)
                {
                    int x = rand.Next(width);
                    int y = rand.Next(height);
                    int size = rand.Next(1, 4);
                    graphics.FillEllipse(Brushes.Gray, x, y, size, size);
                }

                // Add random lines to further obfuscate
                for (int i = 0; i < 10; i++)
                {
                    int x1 = rand.Next(width);
                    int y1 = rand.Next(height);
                    int x2 = rand.Next(width);
                    int y2 = rand.Next(height);
                    using (Pen pen = new Pen(Color.Gray, 2))
                    {
                        graphics.DrawLine(pen, x1, y1, x2, y2);
                    }
                }

                // Set anti-aliasing for smoother text rendering
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                // Define a font for the text
                using (Font font = new Font("Arial", 24, FontStyle.Bold))
                {
                    // Measure the text size to center it in the image
                    SizeF textSize = graphics.MeasureString(password, font);
                    PointF position = new PointF((width - textSize.Width) / 2, (height - textSize.Height) / 2);

                    // Slightly distort the text using a transformation matrix
                    graphics.TranslateTransform(position.X, position.Y);
                    graphics.RotateTransform(rand.Next(-10, 10)); // Random rotation
                    graphics.ScaleTransform(1 + (float)rand.NextDouble() * 0.1f, 1 + (float)rand.NextDouble() * 0.1f); // Random scaling
                    graphics.TranslateTransform(-position.X, -position.Y);

                    // Draw the text onto the image
                    using (Brush textBrush = new SolidBrush(Color.Black))
                    {
                        graphics.DrawString(password, font, textBrush, position);
                    }

                    // Reset the transformation matrix
                    graphics.ResetTransform();
                }
            }

            // Save the image to a memory stream in PNG format
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }


}