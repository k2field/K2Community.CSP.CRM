using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;

//SourceCode.HostServerInterfaces.dll, located by default at C:\Program Files (x86)\K2 blackpearl\Host Server\Bin
using SourceCode.Hosting.Server.Interfaces;

//SourceCode.Logging.dll, located by default at C:\Program Files (x86)\K2 blackpearl\Host Server\Bin
//reference this dll if you want to do any log output as part of your security provider
using SourceCode.Logging;

using K2Community.CSP.CRM.Principals;

namespace K2Community.CSP.CRM
{
    /// <summary>
    /// A custom K2 security provider responsible for interacting with an underlying authentication system or technology.
    /// Provides authentication, user information and membership information
    /// </summary>
    public class SecurityProvider : IHostableSecurityProvider
    {

        #region Class Level Fields

        

        #region Private Fields



        private OrganizationServiceProxy _serviceProxy;

        /// <summary>
        /// Local authInitData variable used to reference the authentication initialization data provided by Init().
        /// </summary>
        private string _authInitData = string.Empty;
        /// <summary>
        /// Local configurationManager variable used to reference the marshaled configuration manager from Init().
        /// </summary>
        private IConfigurationManager _configurationManager = null;
        /// <summary>
        /// Local securityManager variable used to reference the marshaled security manager from Init().
        /// </summary>
        private ISecurityManager _securityManager = null;
        /// <summary>
        /// Local securityLabel variable used to reference the assigned security label from Init().
        /// </summary>
        private string _securityLabel = string.Empty;
        public string SecurityLabel
        {
            get { return _securityLabel; }
        }

        public string URL { get; set; }


        /// <summary>
        /// K2 logging context
        /// </summary>
        private Logger _logger = null;

        #endregion

        #endregion

        #region Default Constructor
        /// <summary>
        /// Instantiates a new SecurityProvider.
        /// </summary>
        public SecurityProvider()
        {
            // No implementation necessary.
            //TODO: Store this somewhere
            this.URL = "http://crm/GeodesysProto12";
        }
        #endregion

        #region Methods

        #region Service Methods

        #region void Init(IServiceMarshalling ServiceMarshalling, IServerMarshaling ServerMarshaling) (IHostableType)
        /// <summary>
        /// Initializes the security provider.
        /// </summary>
        /// <param name="ServiceMarshalling">An IServiceMarshalling representing the service marshaling.</param>
        /// <param name="ServerMarshaling">An IServerMarshaling representing the server marshaling.</param>
        public void Init(IServiceMarshalling ServiceMarshalling, IServerMarshaling ServerMarshaling)
        {
            // Get configuration manager from service marshaling.
            _configurationManager = ServiceMarshalling.GetConfigurationManagerContext();
            // Get security manager from server marshaling.
            _securityManager = ServerMarshaling.GetSecurityManagerContext();
            //set up logging
            this._logger = (Logger)ServiceMarshalling.GetHostedService(typeof(Logger).ToString());
        }
        #endregion

        #region void Init(string label, string authInit)  (IAuthenticationProvider)
        /// <summary>
        /// Initializes the security provider's authentication subsystem. Uses the values set up in the K2 database
        /// (see SQLScriptToRegisterProvider.sql)
        /// </summary>
        /// <param name="label">A string representing the assigned security label.</param>
        /// <param name="authInit">A string representing any additional authentication initialization data.</param>
        public void Init(string label, string authInit)
        {
            _securityLabel = label;
            _authInitData = authInit;
        }
        #endregion

        #region bool RequiresAuthentication() (IHostableSecurityProvider)
        /// <summary>
        /// Determines if authentication is required.
        /// </summary>
        /// <returns>Returns true if the security provider requires authentication, otherwise returns false.</returns>
        public bool RequiresAuthentication()
        {
            //TODO: If this security provider requires that users authenticate, return true, otherwise return false.
            return false;
        }
        #endregion

        #region void Unload()
        /// <summary>
        /// Unloads the security provider and releases any resources held.
        /// </summary>
        public void Unload()
        {
            //TODO: Add clean up code here. Make sure to dispose of any data connections.
            _configurationManager = null;
            _securityManager = null;
        }
        #endregion

        #endregion

        #region Authentication Methods

        #region bool AuthenticateUser(string userName, string password, string extraData)  (IAuthenticationProvider)
        /// <summary>
        /// Authenticates a user.
        /// </summary>
        /// <param name="userName">A string representing the user's username.</param>
        /// <param name="password">A string representing the user's password.</param>
        /// <param name="extraData">string of additional, free-text data that you can use in the authentication method</param>
        /// <returns>Returns true if the user is successfully authenticated, otherwise returns false.</returns>
        public bool AuthenticateUser(string userName, string password, string extraData)
        {
            bool authenticated = false;
            //TODO: Add user authentication code here  and remove the NotImplementedException. 
            //Return true if the user was successfully authenticated, false if the user is not successfully authenticated
            //Return true if the security provider is not used for authentication. do NOT throw a NotImplementException here

            //Note: this method is called when the security provider is used with SSO. the username and password paramters 
            //will be the decrypted SSO-stored username and password that the user entered when caching their SSSO credentials 
            //in K2 workspace

            try
            {
                //sample of logging debug output
                _logger.LogDebugMessage(base.GetType().ToString() + ".AuthenticateUser", "Authenticating: " + userName);

            }
            catch (Exception ex)
            {
                //sample of logging debug output
                _logger.LogErrorMessage(base.GetType().ToString() + ".AuthenticateUser error", "Error: " + ex.Message);
            }
            return authenticated;
        }
        #endregion

        #region string Login(string connectionString) (IAuthenticationProvider)
        /// <summary>
        /// Logs in a user using the provided connection string. Does not need to be implemented: required for backward compatability only
        /// </summary>
        /// <param name="connectionString">A string representing the connection string to use for the login operation.</param>
        /// <returns>A string representing the authenticated user's username.</returns>
        /// <remarks>
        /// Method only included for backward compatibility. Method always throws a NotImplementedException.
        /// </remarks>
        /// <exception cref="System.NotImplementedException" />
        public string Login(string connectionString)
        {
            // Does not need to be implemented unless you use the K2.net 2003 ROM dll
            throw new NotImplementedException();
        }
        #endregion

        #endregion

        #region Group Principal Interaction Methods

        #region IGroupCollection FindGroups(string userName, IDictionary<string, object> properties)  (IRoleProvider)
        /// <summary>
        /// Finds and returns the given user's groups.
        /// </summary>
        /// <param name="userName">A string representing the username of the user whose groups should be returned.</param>
        /// <param name="properties">An IDictionary representing the properties used to filter the groups returned.</param>
        /// <returns>An IGroupCollection representing the groups which were found.</returns>
        public IGroupCollection FindGroups(string userName, IDictionary<string, object> properties)
        {
            //TODO: Add code to retrieve groups for a specific user
            //throw new NotImplementedException(); //if this method is not implemented in your security provider

            //sample code to implement the FindGroups method. 
            //You need to build up a GroupCollection based on the input username
            //add Group objects to the collection and finally return the collection

            //the collection that we will populate and finally return
            GroupCollection groups = new GroupCollection();

            #region CRM code


            bool result;
            try
            {
                //TODO: 
                ServerConnection serverConnect = new ServerConnection();
                
                    ServerConnection.Configuration config = serverConnect.GetServerConfiguration();
                
                if (string.IsNullOrEmpty(this.URL))
                {
                    if (this._logger != null)
                    {
                        this._logger.LogErrorMessage("K2Community.CSP.CRM", "CRM URL not found");
                    }
                    result = false;
                }
                else
                {
                    //////Uri discoveryServiceUri = SecurityProvider.GetDiscoveryServiceUri(this.URL);
                    //////IServiceManagement<IDiscoveryService> serviceManagement = ServiceConfigurationFactory.CreateManagement<IDiscoveryService>(discoveryServiceUri);
                    //////AuthenticationProviderType authenticationType = serviceManagement.get_AuthenticationType();

                    
                    string FetchXml = @"
                    <fetch mapping='logical'>
	                    <entity name='team'>
		                    <attribute name='name' />
	                    </entity>
                    </fetch>";



                    using (_serviceProxy = ServerConnection.GetOrganizationProxy(config))
                    {

                        _serviceProxy.EnableProxyTypes();

                        // Build fetch request and obtain results.
                        Microsoft.Xrm.Sdk.Messages.RetrieveMultipleRequest efr = new Microsoft.Xrm.Sdk.Messages.RetrieveMultipleRequest()
                        {

                            Query = new FetchExpression(FetchXml)
                        };

                        Microsoft.Xrm.Sdk.EntityCollection entityResults = ((Microsoft.Xrm.Sdk.Messages.RetrieveMultipleResponse)_serviceProxy.Execute(efr)).EntityCollection;


                        // Display the results.
                        Console.WriteLine("List all contacts matching specified parameters");
                        Console.WriteLine("===============================================");

                        //sample of logging debug output
                        _logger.LogDebugMessage(base.GetType().ToString() + ".FindGroups", "Finding groups for user: " + userName);

                        foreach (var e in entityResults.Entities)
                        {

                            Console.WriteLine("Team Name: {0}", e.Attributes["name"].ToString());
                            
                            //define a Group object and add it to the collection.
                            //you would probably do this in a for each loop. Here we are just adding two sample groups for demo purposes
                            Group group1 = new Group(this.SecurityLabel, e.Attributes["name"].ToString(), "", "");

                            //add the group to the collection
                            groups.Add(group1);
                        }
                    }


                    //////using (DiscoveryServiceProxy proxy = SecurityProvider.GetProxy<IDiscoveryService, DiscoveryServiceProxy>(serviceManagement, SecurityProvider.GetCredentials(authenticationType, UserName, Password)))
                    //////{
                    //////    proxy.uthenticate();
                    //////}
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (this._logger != null)
                {
                    //sample of logging debug output
                    _logger.LogErrorMessage(base.GetType().ToString() + ".FindGroups error", "User: " + userName + " Error: " + ex.Message);
           
                    this._logger.LogErrorMessage("CRM5OnlineProvider", ex.Message);
                }
                result = false;
            }

            #endregion






            //return the collection of groups that the user belongs to
            return groups;
        }
        #endregion

        #region IGroupCollection FindGroups(string userName, IDictionary<string, object> properties, string extraData)  (IRoleProvider)
        /// <summary>
        /// Finds and returns the groups for a specific user
        /// </summary>
        /// <param name="userName">A string representing the username of the user whose groups should be returned.</param>
        /// <param name="properties">An IDictionary representing the properties used to filter the groups returned.</param>
        /// <param name="extraData">A string representing any extra data which may which may be needed.</param>
        /// <returns>An IGroupCollection representing the groups which were found.</returns>
        public IGroupCollection FindGroups(string userName, IDictionary<string, object> properties, string extraData)
        {
            //TODO: Add group retrieval code for a specific user,  using extradata
            //throw new NotImplementedException(); //if this method is not implemented in your security provder

            //if necessary, use the extraData parameter to perform additional processing
            return FindGroups(userName, properties);
        }
        #endregion

        #region IGroup GetGroup(string name)  (IRoleProvider)
        /// <summary>
        /// Gets a group.
        /// </summary>
        /// <param name="name">A string representing the name of the group to retrieve.</param>
        /// <returns>Returns an IGroup representing the group if the group is found, otherwise returns null.</returns>
        public IGroup GetGroup(string name)
        {
            //TODO: Add group retrieval code here and remove the NotImplementedException
            //throw new NotImplementedException(); //if this method is not implemented in your security provider
            Group group = null;
            try
            {
                //sample of logging debug output
                _logger.LogDebugMessage(base.GetType().ToString() + ".GetGroup", "Group: " + name);

                //TODO: Instantiate the group object and set properties. Here we are just adding a sample group for demo purposes
                group = new Group(this.SecurityLabel, "SAMPLE Group1Name", "SAMPLE Group1Description", "group1@sample.com");
            }
            catch (Exception ex)
            {
                //sample of logging debug output
                _logger.LogErrorMessage(base.GetType().ToString() + ".GetGroup error", "Group: " + name + " Error: " + ex.Message);
            }

            return group;    
        }
        #endregion

        #region IGroup GetGroup(string name, string extraData)  (IRoleProvider)
        /// <summary>
        ///  Gets a single group using the Name parameter along with free-form extra data
        /// </summary>
        /// <param name="name">A string representing the name of the group to retrieve.</param>
        /// <param name="extraData">A string representing any extra data which may which may be needed.</param>
        /// <returns>Returns an IGroup representing the group if the group is found, otherwise returns null.</returns>
        public IGroup GetGroup(string name, string extraData)
        {
            //TODO: Add group retrieval code here and remove the NotImplementedException
            //use the extra data to perform any additional processing that may be required
            //throw new NotImplementedException(); //if this method is not implemented in your security provder

            return GetGroup(name);
        }
        #endregion

        #region Dictionary<string, string> QueryGroupProperties()  (IRoleProvider)
        /// <summary>
        /// Retrieves the common properties and their types for a Group. 
        /// This method is only used to list the possible Properties for a Group
        /// </summary>
        /// <returns>A Dictionary containing the general group properties.</returns>
        public Dictionary<string, string> QueryGroupProperties()
        {
            return K2Utils.K2GroupPropertyDefinitions;
        }
        #endregion

        #endregion

        #region User Principal Interaction Methods

        #region IUserCollection FindUsers(string groupName, IDictionary<string, object> properties)  (IRoleProvider)
        /// <summary>
        /// Finds and returns the given group's users.
        /// </summary>
        /// <param name="groupName">A string representing the group name of the group for which users should be returned. Could include wildcards (Starts with: *xyz, End with: xyz*, Contains: *xyz*)</param>
        /// <param name="properties">An IDictionary representing the properties used to filter the users returned. You can add additional filter properties to this collection</param>
        /// <returns>An IUserCollection representing the users which were found.</returns>
        public IUserCollection FindUsers(string groupName, IDictionary<string, object> properties)
        {
            //TODO: Add the code to retrieve a collection of users based on a group name and remove the NotImplementedException
            //throw new NotImplementedException(); //if your security provider does not implement this method

            //NOTE: The wildcards for the group name passed in by K2 are
            //   Starts with:   *xyz
            //   End with:      xyz*
            //   Contains:      *xyz*
            
            //sample code to implement the FindUsers method. 
            //You need to build up a UserCollection based on the input groupname
            //add User objects to the collection and finally return the collection

            //the colleciton we will populate and finally return
            UserCollection users = new UserCollection();

            try
            {
                //sample of logging debug output
                _logger.LogDebugMessage(base.GetType().ToString() + ".FindUsers", "Group: " + groupName);
                //TODO: 
                ServerConnection serverConnect = new ServerConnection();

                ServerConnection.Configuration config = serverConnect.GetServerConfiguration();

                using (_serviceProxy = ServerConnection.GetOrganizationProxy(config))
                {

                    _serviceProxy.EnableProxyTypes();
                    // Obtain the Organization Context.
                    OrganizationServiceContext context = new OrganizationServiceContext(_serviceProxy);

                    // Create Linq Query.
                    var teams = (from t in context.CreateQuery<Team>()
                                 where t.Name == groupName
                                 select t.TeamId);

                    Guid? lastTeamID = new Guid();

                    // Display results.
                    foreach (var team in teams)
                    {
                        //Console.WriteLine("Linq Retrieved: {0}", team);
                        lastTeamID = team;
                        var teamMembers = (from u in context.CreateQuery<SystemUser>()
                                           join s in context.CreateQuery<TeamMembership>() on u.SystemUserId equals s.SystemUserId
                                           where s.TeamId == lastTeamID
                                           orderby u.DomainName
                                           select u.DomainName);

                        // Display results.
                        foreach (var user in teamMembers)
                        {
                            Console.WriteLine("Linq Retrieved: {0} i n {1}", user, team);
                            User user1 = new User("K2", user, "", "user1@sample.com", "", "", "");

                            users.Add(user1);
                        }

                    }


                    //define a User object and add it to the collection.
                    //you would probably do this in a for each loop. Here we are just adding two sample users for demo purposes
                   
                    //add users to the collection
                }
            }
            catch (Exception ex)
            {
                //sample of logging debug output
                _logger.LogErrorMessage(base.GetType().ToString() + ".FindUsers error", "Group: " + groupName + " Error: " + ex.Message);
            }

           //return the collection of users for the group
            return users;

        }
        #endregion

        #region IUserCollection FindUsers(string groupName, IDictionary<string, object> properties, string extraData)  (IRoleProvider)
        /// <summary>
        /// Finds and returns the given group's users. Extends the standard FindUsers method by adding a free-form ExtraData
        /// value that you can use in your own code.
        /// </summary>
        /// <param name="groupName">A string representing the group name of the group for which users should be returned. Could include wildcards</param>
        /// <param name="properties">An IDictionary representing the properties used to filter the users returned. You can add additional filter properties to this collection</param>
        /// <param name="extraData">A string representing any extra data which may which may be needed.</param>
        /// <returns>An IUserCollection representing the users which were found.</returns>
        public IUserCollection FindUsers(string groupName, IDictionary<string, object> properties, string extraData)
        {
            //TODO: Add user retrieval code here and remove the NotImplementedException
            //throw new NotImplementedException(); //if your security provider does not implement this method

            //if necessary, use the extraData parameter to perform additional processing
            return FindUsers(groupName, properties);
        }
        #endregion

        #region IUser GetUser(string name)  (IRoleProvider)
        /// <summary>
        /// Gets a single user using the Name parameter to locate the user.
        /// </summary>
        /// <param name="name">A string representing the name of the user to retrieve.</param>
        /// <returns>Returns an IUser representing the user if the user is found, otherwise returns null.</returns>
        public IUser GetUser(string name)
        {
            //TODO: Add user retrieval code here and remove the NotImplementedException
            //throw new NotImplementedException(); //if your security provider does not implement this method

            User user = null;
            try
            {
                //sample of logging debug output
                _logger.LogDebugMessage(base.GetType().ToString() + ".GetUser", "User: " + name);

                //TODO: Instantiate the user object and set properties. 
                //Here we are just returning a sampel user for demonstration purposes
                user = new User(SecurityLabel, "SAMPLE User1Name", "SAMPLE User1Description", "user1@sample.com", "SAMPLE User1Manager", "SAMPLE User1SipAccount", "SAMPLE User1DisplayName");                ;
            }
            catch (Exception ex)
            {
                //sample of logging debug output
                _logger.LogErrorMessage(base.GetType().ToString() + ".GetUser error", "User: " + name + " Error: " + ex.Message);
            }

            
            return user;    
        }
        #endregion

        #region IUser GetUser(string name, string extraData)  (IRoleProvider)
        /// <summary>
        /// Gets a single user using the Name parameter along with extra data to locate the user.
        /// </summary>
        /// <param name="name">A string representing the name of the user to retrieve.</param>
        /// <param name="extraData">A string representing any extra data which may which may be needed.</param>
        /// <returns>Returns an IUser representing the user if the user is found, otherwise returns null.</returns>
        public IUser GetUser(string name, string extraData)
        {
            //TODO: Add user retrieval code here and remove the NotImplementedException
            //throw new NotImplementedException(); //if your security provider does not implement this method

            return GetUser(name);
        }
        #endregion

        #region Dictionary<string, string> QueryUserProperties()  (IRoleProvider)
        /// <summary>
        /// Retrieves the common properties and their types for a User. 
        /// This method is only used to list the possible Properties for a user object
        /// </summary>
        /// <returns>A Dictionary containing the general user properties.</returns>
        public Dictionary<string, string> QueryUserProperties()
        {
            return K2Utils.K2UserPropertyDefinitions;
        }
        #endregion

        #endregion

        #region Utility Methods

        #region crm methods

        internal static Uri GetDiscoveryServiceUri(string serverName)
        {
            string arg = "/XRMServices/2011/Discovery.svc";
            return new Uri(string.Format("{0}{1}", serverName, arg));
        }


        //////// SourceCode.Security.Providers.CRM5Online.CRM5OnlineProvider
        //////private static TProxy GetProxy<TService, TProxy>(IServiceManagement<TService> serviceManagement, AuthenticationCredentials authCredentials)
        //////    where TService : class
        //////    where TProxy : ServiceProxy<TService>
        //////{
        //////    Type typeFromHandle = typeof(TProxy);
        //////    if (serviceManagement.get_AuthenticationType() != 1)
        //////    {
        //////        AuthenticationCredentials authenticationCredentials = serviceManagement.Authenticate(authCredentials);
        //////        return (TProxy)((object)typeFromHandle.GetConstructor(new Type[]
        //////        {
        //////            typeof(IServiceManagement<TService>),
        //////            typeof(SecurityTokenResponse)
        //////        }).Invoke(new object[]
        //////        {
        //////            serviceManagement,
        //////            authenticationCredentials.get_SecurityTokenResponse()
        //////        }));
        //////    }
        //////    return (TProxy)((object)typeFromHandle.GetConstructor(new Type[]
        //////    {
        //////        typeof(IServiceManagement<TService>),
        //////        typeof(ClientCredentials)
        //////    }).Invoke(new object[]
        //////    {
        //////        serviceManagement,
        //////        authCredentials.get_ClientCredentials()
        //////    }));
        //////}

        #endregion

        #region string FormatItemName(string name) (IRoleProvider)
        /// <summary>
        /// Formats an item's name.
        /// </summary>
        /// <param name="name">A string representing the item name to format.</param>
        /// <returns>A string representing the formatted item name.</returns>
        public string FormatItemName(string name)
        {
            // Add item name formatting code here, if required
            //throw new NotImplementedException(); //if your security provider does not implement this method
            return name;
        }
        #endregion

        #region ArrayList ResolveQueue(string data)
        /// <summary>
        /// Resolves the queue based on the provided queue data. Not implemented.
        /// </summary>
        /// <param name="data">A string representing the queue data to resolve.</param>
        /// <returns>An ArrayList representing the resolved queue data.</returns>
        /// <remarks>
        /// Method included for backward compatibility. Method always throws a NotImplementedException.
        /// </remarks>
        /// <exception cref="System.NotImplementedException" />
        public ArrayList ResolveQueue(string data)
        {
            //throw new NotImplementedException(); //if your security provider does not implement this method
            try
            {
                //sample of logging debug output
                _logger.LogDebugMessage(base.GetType().ToString() + ".ResolveQueue", "Data: " + data);
            }
            catch (Exception ex)
            {
                //sample of logging error output
                _logger.LogErrorMessage(base.GetType().ToString() + ".ResolveQueue error", "Data: " + data + " Error: " + ex.Message);
            }
            return null;
        }
        #endregion

        #endregion

        #endregion
    }
}