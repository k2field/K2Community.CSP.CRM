using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
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
    public class SecurityProvider : IHostableSecurityProvider, IHostableType, IAuthenticationProvider, IRoleProvider
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

        private string _crmconfigurations;
//        public string CRMConfigs { get; set; }

        public string TargetRoleProviderLabel { get; set; }

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
            Console.WriteLine("CRM SecurityProvider {0}", "SecurityProvider constructing");
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
            try
            {
                _securityLabel = label;
                _authInitData = authInit;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(authInit);
                this._crmconfigurations = xmlDocument.SelectSingleNode("AuthInit/init/Configurations").OuterXml;
            }
            catch (Exception ex)
            {
                //sample of logging debug output
                _logger.LogErrorMessage(base.GetType().ToString(), string.Format("{0}.Init error label:'{1}' authInit:'{2}' {3}{4}", base.GetType().ToString(), label, authInit , ex.Message, ex.StackTrace));
            }
        }
        #endregion

        #region void Init(string label, string roleInit)  (IRoleProvider)
        /// <summary>
        /// Initializes the role provider. Uses the values set up in the K2 database
        /// (see SQLScriptToRegisterProvider.sql)
        /// </summary>
        /// <param name="label">A string representing the assigned security label.</param>
        /// <param name="roleInit">A string representing any additional authentication initialization data.</param>
        void IRoleProvider.Init(string label, string roleInit)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(roleInit);
                this._crmconfigurations = xmlDocument.SelectSingleNode("roleprovider/init/Configurations").OuterXml;
                this.TargetRoleProviderLabel = label;
            }
            catch (Exception ex)
            {
                //sample of logging debug output
                _logger.LogErrorMessage(base.GetType().ToString(), string.Format("{0}.IRoleProvider.Init error label:'{1}' authInit:'{2}' {3}{4}", base.GetType().ToString(), label, roleInit, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        #region bool RequiresAuthentication() (IHostableSecurityProvider)
        /// <summary>
        /// Determines if authentication is required.
        /// </summary>
        /// <returns>Returns true if the security provider requires authentication, otherwise returns false.</returns>
        public bool RequiresAuthentication()
        {
            //If this security provider requires that users authenticate, return true, otherwise return false.
            return false;
        }
        #endregion

        #region void Unload()
        /// <summary>
        /// Unloads the security provider and releases any resources held.
        /// </summary>
        public void Unload()
        {
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
            bool authenticated = true;

            //Return true if the user was successfully authenticated, false if the user is not successfully authenticated
            //Return true if the security provider is not used for authentication. do NOT throw a NotImplementException here
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
            //the collection that we will populate and finally return
            GroupCollection groups = new GroupCollection();

            #region CRM code

            try
            {
                ServerConnection serverConnect = new ServerConnection(this._crmconfigurations);
                ServerConnection.Configuration config = serverConnect.GetServerConfiguration();
                if (config == null)
                {
                    if (this._logger != null)
                    {
                        this._logger.LogErrorMessage("K2Community.CSP.CRM", "CRM URL not found");
                    }
                }
                else
                {
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

                        //sample of logging debug output
                        _logger.LogDebugMessage(base.GetType().ToString() + ".FindGroups", "Finding groups for user: " + userName);
                        foreach (var e in entityResults.Entities)
                        {
                            _logger.LogDebugMessage(base.GetType().ToString() + ".FindGroups", "Team Name: {0}", e.Attributes["name"].ToString());
                            Group group1 = new Group(this.SecurityLabel, e.Attributes["name"].ToString(), "", "");

                            //add the group to the collection
                            groups.Add(group1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (this._logger != null)
                {
                    _logger.LogErrorMessage(base.GetType().ToString() + ".FindGroups error", "User: " + userName + " Error: " + ex.Message + ex.StackTrace);
                }
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
            Group group = null;
            try
            {
                //sample of logging debug output
                _logger.LogDebugMessage(base.GetType().ToString() + ".GetGroup", "Group: " + name);

                //TODO: Get more information from CRM
                group = new Group(this.SecurityLabel, name, "SAMPLE Group1Description", "group1@sample.com");
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
            //TODO: Wildcard search
            //NOTE: The wildcards for the group name passed in by K2 are
            //   Starts with:   *xyz
            //   End with:      xyz*
            //   Contains:      *xyz*
            
            //the colleciton we will populate and finally return
            UserCollection users = new UserCollection();

            try
            {
                _logger.LogDebugMessage(base.GetType().ToString() + ".FindUsers", "Group: " + groupName);
                ServerConnection serverConnect = new ServerConnection(this._crmconfigurations);
                ServerConnection.Configuration config = serverConnect.GetServerConfiguration();
                if (config == null)
                {
                    if (this._logger != null)
                    {
                        this._logger.LogErrorMessage("K2Community.CSP.CRM", "CRM URL not found");
                    }
                }
                else
                {
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
                                _logger.LogDebugMessage("FindUser",string.Format("Linq Retrieved: {0} i n {1}", user, team));
                                User user1 = new User("K2", user, "", "user1@sample.com", "", "", "");
                                users.Add(user1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogErrorMessage(base.GetType().ToString() + ".FindUsers error", "Group: " + groupName + " Error: " + ex.Message + ex.StackTrace);
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
            //Users are all in AD
            throw new NotImplementedException(); //if your security provider does not implement this method

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