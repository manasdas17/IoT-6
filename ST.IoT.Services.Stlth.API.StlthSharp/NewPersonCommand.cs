using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public class NewPersonCommand : StlthCommand
    {
        public PersonRequestBuilder Parent { get; private set; }
        public string EmailAddress { get; private set; }
        public string Password { get; private set; }

        public Task<NewPersonCommandResult> ResultAsync => this.executeAsync();
        public NewPersonCommandResult Result => this.execute();

        public NewPersonCommand(PersonRequestBuilder parent, string emailAddress = null, string password = null)
        {
            Parent = parent;
            EmailAddress = emailAddress;
            Password = password;
        }

        protected override string createCommandBody()
        {
            return "";
        }

        public NewPersonCommandResult execute()
        {
            return executeAsync().Result;
        }

        private async Task<NewPersonCommandResult> executeAsync()
        {
            var sb = new StringBuilder();
            if (EmailAddress != null) append(sb, "'EmailAddress': '" + EmailAddress + "'");
            if (Password != null) append(sb, "'Password': '" + Password + "'");

            var response = await StlthRestRequestExecutor.executeAsync(
                method: Method.POST,
                requestUrl: "/Person",
                body: "{" + sb.ToString() + "}");

            var result = new NewPersonCommandResult(response);
            return result;
        }

        private void append(StringBuilder sb, string content)
        {
            if (sb.Length > 0) sb.Append(",");
            sb.Append(content);
        }
    }
}