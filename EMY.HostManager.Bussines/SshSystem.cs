

using System.Diagnostics;

namespace EMY.HostManager.Bussines
{
    public class SshSystem
    {
        public string DestinationAdress { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        private string _password;
        public string Password { set { _password = value; } }

        public SshSystem(string destinationAdress, string userName, string password, int port = 22)
        {
            this.DestinationAdress = destinationAdress;
            this.UserName = userName;
            this.Password = password;
            this.Port = port;
        }

        public bool CheckConnection()
        {
            Chilkat.Ssh ssh = new Chilkat.Ssh();

            bool success = ssh.Connect(DestinationAdress, Port);
            if (success != true)
            {
                Debug.WriteLine(ssh.LastErrorText);
                return false;
            }

            // Wait a max of 5 seconds when reading responses..
            ssh.IdleTimeoutMs = 5000;

            // Authenticate using login/password:
            success = ssh.AuthenticatePw(UserName, _password);
            if (success != true)
            {
                Debug.WriteLine(ssh.LastErrorText);
                return false;
            }

            return success;
        }

        public bool UploadWithScp(string sourceFileName, string destFileName)
        {
            Chilkat.Ssh ssh = new Chilkat.Ssh();

            bool success = ssh.Connect(DestinationAdress, Port);
            if (success != true)
            {
                Debug.WriteLine(ssh.LastErrorText);
                return false;
            }

            // Wait a max of 5 seconds when reading responses..
            ssh.IdleTimeoutMs = 5000;

            // Authenticate using login/password:
            success = ssh.AuthenticatePw(UserName, _password);
            if (success != true)
            {
                Debug.WriteLine(ssh.LastErrorText);
                return false;
            }

            // Once the SSH object is connected and authenticated, we use it
            // as the underlying transport in our SCP object.
            Chilkat.Scp scp = new Chilkat.Scp();

            success = scp.UseSsh(ssh);
            if (success != true)
            {
                Debug.WriteLine(scp.LastErrorText);
                return false;
            }

            success = scp.UploadFile(sourceFileName, destFileName);
            if (success != true)
            {
                Debug.WriteLine(scp.LastErrorText);
                return false;
            }

            Debug.WriteLine("SCP upload file success.");

            // Disconnect
            ssh.Disconnect();

            return true;
        }

        public bool RunSSHCode(string Code, ref string resultmessage)
        {
            Chilkat.Ssh ssh = new Chilkat.Ssh();

            bool success = ssh.Connect(DestinationAdress, Port);
            if (success != true)
            {
                Debug.WriteLine(ssh.LastErrorText);
                return false;
            }

            // Wait a max of 5 seconds when reading responses..
            ssh.IdleTimeoutMs = 5000;

            // Authenticate using login/password:
            success = ssh.AuthenticatePw(UserName, _password);
            if (success != true)
            {
                Debug.WriteLine(ssh.LastErrorText);
                return false;
            }

            //  Send some commands and get the output.
            resultmessage = ssh.QuickCommand(Code, "ansi");
            if (ssh.LastMethodSuccess != true)
            {
                Debug.WriteLine(ssh.LastErrorText);
                return false;
            }
            Debug.WriteLine(resultmessage);


            return true;
        }

        public bool UploadStringWithSCP(string content, string destFileName)
        {

            Chilkat.Ssh ssh = new Chilkat.Ssh();
            bool success = ssh.Connect(DestinationAdress, Port);
            if (success != true)
            {
                Debug.WriteLine(ssh.LastErrorText);
                return false;
            }

            // Wait a max of 5 seconds when reading responses..
            ssh.IdleTimeoutMs = 5000;

            // Authenticate using login/password:
            success = ssh.AuthenticatePw(UserName, _password);
            if (success != true)
            {
                Debug.WriteLine(ssh.LastErrorText);
                return false;
            }

            // Once the SSH object is connected and authenticated, we use it
            // as the underlying transport in our SCP object.
            Chilkat.Scp scp = new Chilkat.Scp();

            success = scp.UseSsh(ssh);
            if (success != true)
            {
                Debug.WriteLine(scp.LastErrorText);
                return false;
            }

            // The utf-8 byte representation of the string will be uploaded.
            // See https://www.chilkatsoft.com/p/p_463.asp for a list of valid charsets.
            string charset = "utf-8";

            // This uploads to the "uploads/text" directory relative to the HOME
            // directory of the SSH user account.  
            // Note: The remote target directory must already exist on the SSH server.
            success = scp.UploadString(destFileName, content, charset);
            if (success != true)
            {
                Debug.WriteLine(scp.LastErrorText);
                return false;
            }

            Debug.WriteLine("SCP upload string success.");

            // Disconnect
            ssh.Disconnect();

            return true;
        }

    }
}
