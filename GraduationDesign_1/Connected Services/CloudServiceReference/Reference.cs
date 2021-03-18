﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace GraduationDesign_1.CloudServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CloudServiceReference.ICryptoService", SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface ICryptoService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/Login", ReplyAction="http://tempuri.org/ICryptoService/LoginResponse")]
        int Login(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/Login", ReplyAction="http://tempuri.org/ICryptoService/LoginResponse")]
        System.Threading.Tasks.Task<int> LoginAsync(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/AddUser", ReplyAction="http://tempuri.org/ICryptoService/AddUserResponse")]
        bool AddUser(string name, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/AddUser", ReplyAction="http://tempuri.org/ICryptoService/AddUserResponse")]
        System.Threading.Tasks.Task<bool> AddUserAsync(string name, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/showUsers", ReplyAction="http://tempuri.org/ICryptoService/showUsersResponse")]
        string[] showUsers();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/showUsers", ReplyAction="http://tempuri.org/ICryptoService/showUsersResponse")]
        System.Threading.Tasks.Task<string[]> showUsersAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/createFile", ReplyAction="http://tempuri.org/ICryptoService/createFileResponse")]
        void createFile(string FilePath, string userName, string downloadName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/createFile", ReplyAction="http://tempuri.org/ICryptoService/createFileResponse")]
        System.Threading.Tasks.Task createFileAsync(string FilePath, string userName, string downloadName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/choseFile", ReplyAction="http://tempuri.org/ICryptoService/choseFileResponse")]
        string choseFile(string FilePath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/choseFile", ReplyAction="http://tempuri.org/ICryptoService/choseFileResponse")]
        System.Threading.Tasks.Task<string> choseFileAsync(string FilePath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/DisplayFiles", ReplyAction="http://tempuri.org/ICryptoService/DisplayFilesResponse")]
        string[] DisplayFiles(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/DisplayFiles", ReplyAction="http://tempuri.org/ICryptoService/DisplayFilesResponse")]
        System.Threading.Tasks.Task<string[]> DisplayFilesAsync(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/FindFile", ReplyAction="http://tempuri.org/ICryptoService/FindFileResponse")]
        string[] FindFile(string file, string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/FindFile", ReplyAction="http://tempuri.org/ICryptoService/FindFileResponse")]
        System.Threading.Tasks.Task<string[]> FindFileAsync(string file, string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/DecryptFile", ReplyAction="http://tempuri.org/ICryptoService/DecryptFileResponse")]
        void DecryptFile(string filename, string username, string downloadPath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICryptoService/DecryptFile", ReplyAction="http://tempuri.org/ICryptoService/DecryptFileResponse")]
        System.Threading.Tasks.Task DecryptFileAsync(string filename, string username, string downloadPath);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICryptoServiceChannel : GraduationDesign_1.CloudServiceReference.ICryptoService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CryptoServiceClient : System.ServiceModel.ClientBase<GraduationDesign_1.CloudServiceReference.ICryptoService>, GraduationDesign_1.CloudServiceReference.ICryptoService {
        
        public CryptoServiceClient() {
        }
        
        public CryptoServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CryptoServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CryptoServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CryptoServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int Login(string username, string password) {
            return base.Channel.Login(username, password);
        }
        
        public System.Threading.Tasks.Task<int> LoginAsync(string username, string password) {
            return base.Channel.LoginAsync(username, password);
        }
        
        public bool AddUser(string name, string password) {
            return base.Channel.AddUser(name, password);
        }
        
        public System.Threading.Tasks.Task<bool> AddUserAsync(string name, string password) {
            return base.Channel.AddUserAsync(name, password);
        }
        
        public string[] showUsers() {
            return base.Channel.showUsers();
        }
        
        public System.Threading.Tasks.Task<string[]> showUsersAsync() {
            return base.Channel.showUsersAsync();
        }
        
        public void createFile(string FilePath, string userName, string downloadName) {
            base.Channel.createFile(FilePath, userName, downloadName);
        }
        
        public System.Threading.Tasks.Task createFileAsync(string FilePath, string userName, string downloadName) {
            return base.Channel.createFileAsync(FilePath, userName, downloadName);
        }
        
        public string choseFile(string FilePath) {
            return base.Channel.choseFile(FilePath);
        }
        
        public System.Threading.Tasks.Task<string> choseFileAsync(string FilePath) {
            return base.Channel.choseFileAsync(FilePath);
        }
        
        public string[] DisplayFiles(string userName) {
            return base.Channel.DisplayFiles(userName);
        }
        
        public System.Threading.Tasks.Task<string[]> DisplayFilesAsync(string userName) {
            return base.Channel.DisplayFilesAsync(userName);
        }
        
        public string[] FindFile(string file, string userName) {
            return base.Channel.FindFile(file, userName);
        }
        
        public System.Threading.Tasks.Task<string[]> FindFileAsync(string file, string userName) {
            return base.Channel.FindFileAsync(file, userName);
        }
        
        public void DecryptFile(string filename, string username, string downloadPath) {
            base.Channel.DecryptFile(filename, username, downloadPath);
        }
        
        public System.Threading.Tasks.Task DecryptFileAsync(string filename, string username, string downloadPath) {
            return base.Channel.DecryptFileAsync(filename, username, downloadPath);
        }
    }
}