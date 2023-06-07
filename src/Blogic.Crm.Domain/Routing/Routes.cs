namespace Blogic.Crm.Domain.Routing;

/// <summary>
///     Contains the controller endpoint routes.
/// </summary>
public sealed class Routes
{
    /// <summary>
    ///     Endpoint root route.
    /// </summary>
    public const string Root = "pages";
    
    /// <summary>
    ///     Controller versions.
    /// </summary>
    public const string Version = "v1";
    
    /// <summary>
    ///     The base of the controller endpoint routes.
    /// </summary>
    public const string Base = $"{Root}/{Version}";

    /// <summary>
    ///     Controller endpoint paths connected to the client.
    /// </summary>
    public static class Client
    {
        /// <summary>
        ///     The base of the client controller endpoint routes.
        /// </summary>
        public const string ClientBase = Base + "/client";
        
        /// <summary>
        ///     Controller endpoint for client deletion.
        /// </summary>
        public const string Delete = ClientBase + "/{id:long}/account/delete";
        
        /// <summary>
        ///     Controller endpoint for client update.
        /// </summary>
        public const string Update = ClientBase + "/{id:long}/account/update";
        
        /// <summary>
        ///     Controller endpoint for client creation.
        /// </summary>
        public const string Create = ClientBase + "/account/create";
        
        /// <summary>
        ///     Controller endpoint for client records retrieval.
        /// </summary>
        public const string GetAll = ClientBase + "/dashboard";
        
        /// <summary>
        ///     Controller endpoint for client record retrieval.
        /// </summary>
        public const string Get = ClientBase + "/{id:long}/account";
        
        /// <summary>
        ///     Controller endpoint for client exportation.
        /// </summary>
        public const string Export = ClientBase + "/dashboard/export";
    }

    /// <summary>
    ///     Controller endpoint paths connected to the consultant.
    /// </summary>
    public static class Consultant
    {
        /// <summary>
        ///     The base of the consultant controller endpoint routes.
        /// </summary>
        public const string ConsultantBase = Base + "/consultant";

        /// <summary>
        ///     Controller endpoint for consultant deletion.
        /// </summary>
        public const string Delete = ConsultantBase + "/{id:long}/account/delete";
        
        /// <summary>
        ///     Controller endpoint for consultant update.
        /// </summary>
        public const string Update = ConsultantBase + "/{id:long}/account/update";
        
        /// <summary>
        ///     Controller endpoint for consultant creation.
        /// </summary>
        public const string Create = ConsultantBase + "/account/create";
        
        /// <summary>
        ///     Controller endpoint for consultant records retrieval.
        /// </summary>
        public const string GetAll = ConsultantBase + "/dashboard";
        
        /// <summary>
        ///     Controller endpoint for consultant record retrieval.
        /// </summary>
        public const string Get = ConsultantBase + "/{id:long}/account";
        
        /// <summary>
        ///     Controller endpoint for consultant exportation.
        /// </summary>
        public const string Export = ConsultantBase + "/dashboard/export";
    }

    /// <summary>
    ///     Controller endpoint paths connected to the contract.
    /// </summary>
    public static class Contract
    {
        /// <summary>
        ///     The base of the contract controller endpoint routes.
        /// </summary>
        public const string ContractBase = Base + "/contract";

        /// <summary>
        ///     Controller endpoint for contract deletion.
        /// </summary>
        public const string Delete = ContractBase + "/{id:long}/account/delete";
        
        /// <summary>
        ///     Controller endpoint for contract update.
        /// </summary>
        public const string Update = ContractBase + "/{id:long}/account/update";
        
        /// <summary>
        ///     Controller endpoint for contract creation.
        /// </summary>
        public const string Create = ContractBase + "/account/create";
        
        /// <summary>
        ///     Controller endpoint for contract records retrieval.
        /// </summary>
        public const string GetAll = ContractBase + "/dashboard";
        
        /// <summary>
        ///     Controller endpoint for contract record retrieval.
        /// </summary>
        public const string Get = ContractBase + "/{id:long}/account";
        
        /// <summary>
        ///     Controller endpoint for contract exportation.
        /// </summary>
        public const string Export = ContractBase + "/dashboard/export";
    }
}