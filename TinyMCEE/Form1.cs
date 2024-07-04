using Microsoft.Web.WebView2.Core;

namespace TinyMCEE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            var env = await CoreWebView2Environment.CreateAsync(null, null, new CoreWebView2EnvironmentOptions("--remote-debugging-port=9222"));
            await webView21.EnsureCoreWebView2Async(env);
            LoadTinyMCE();
        }


        private void LoadTinyMCE()
        {
            string htmlFilePath = Path.Combine(Application.StartupPath, "editor.html");

            if (File.Exists(htmlFilePath))
            {
                string htmlContent = File.ReadAllText(htmlFilePath);
                string dataFromDatabase = GetDataFromDatabase();

                // Escape the data to be safely embedded in JavaScript
                string escapedData = EscapeJavaScriptString(dataFromDatabase);

                // Add a script tag to call the initTinyMCE function with the data
                string script = $"<script>window.onload = function() {{ initTinyMCE('{escapedData}'); }};</script>";

                // Inject the script tag just before the closing </body> tag
                string injectedHtml = htmlContent.Replace("</body>", script + "</body>");

                // Create a temporary file with the injected content
                string tempFilePath = Path.Combine(Application.StartupPath, "temp_editor.html");
                File.WriteAllText(tempFilePath, injectedHtml);

                // Load the temporary file
                webView21.Source = new Uri(tempFilePath);
            }
            else
            {
                MessageBox.Show("HTML file not found: " + htmlFilePath);
            }
        }

        private string GetDataFromDatabase()
        {
            // Replace with your actual database fetching logic
            return "This is the data fetched from the database.";
        }

        private string EscapeJavaScriptString(string input)
        {
            return input.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"");
        }
    }
}
