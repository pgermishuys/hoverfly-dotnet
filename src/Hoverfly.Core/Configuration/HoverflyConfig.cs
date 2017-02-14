﻿namespace Hoverfly.Core.Configuration
{
    using System;

    public class HoverflyConfig
    {
        private const int DEFAULT_PROXY_PORT = 8500;
        private const int DEFAULT_ADMIN_PORT = 8888;

        private const string LOCALHOST = "http://localhost";

        /// <summary>
        /// Gets the admin port number used by hoverfly.
        /// </summary>
        public int AdminPort { get; private set; } = DEFAULT_PROXY_PORT;

        /// <summary>
        /// Gets the proxy port number used by hoverfly.
        /// </summary>
        public int ProxyPort { get; private set; } = DEFAULT_ADMIN_PORT;

        /// <summary>
        /// Gets if the hoverfly uses a remote instance.
        /// </summary>
        /// <remarks>If set to true, no hoverfly process will be started by .Net.</remarks>
        public bool IsRemoteInstance { get; private set; }

        /// <summary>
        /// Gets the URL of the remote hoverfly intance.
        /// </summary>
        public Uri RemoteHost { get; private set; } = new Uri(LOCALHOST);

        /// <summary>
        /// Gets if any request to localhost will be or not be proxied throigh hoverfly..
        /// </summary>
        public bool ProxyLocalhost { get; private set; }

        /// <summary>
        /// Gets the base path to the hoverfly.exe.
        /// </summary>
        public string HoverflyBasePath { get; private set; } = string.Empty;

        /// <summary>
        /// Create an new instance of <see cref="HoverflyConfig"/>.
        /// </summary>
        /// <returns>Returns an instance of <see cref="HoverflyConfig"/>.</returns>
        public static HoverflyConfig Config() => new HoverflyConfig();

        ///<summary>Use this method if there is already a remote instance of hoverfly running. By using this method .Net will not start a hoverfly instance.</summary>
        ///<param name="remoteHost">The URL of the romote hoverfly instance. If nothing is specified, localhost, will be used by default.</param>
        ///<param name="proxyPort">The proxy port the remote hoverfly uses. If nothing is specified, 8500, will be used by default.</param>
        ///<param name="adminPort">The admin port the remote hoverfly uses. If nothing is specified, 8888, will be used by default.</param>
        ///<returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig UseRemoteInstance(
            Uri remoteHost = null,
            int proxyPort = DEFAULT_PROXY_PORT,
            int adminPort = DEFAULT_ADMIN_PORT)
        {
            this.IsRemoteInstance = true;
            RemoteHost = remoteHost ?? new Uri(LOCALHOST);
            return this;
        }


        /// <summary>
        /// Sets the proxy port used by the hoverfly.
        /// </summary>
        /// <param name="port">The proxy port.</param>
        /// <returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig SetProxyPort(int port)
        {
            ProxyPort = port;
            return this;
        }

        /// <summary>
        /// Sets the admin used by the hoverfly.
        /// </summary>
        /// <param name="port">The admin port.</param>
        /// <returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig SetAdminPort(int port)
        {
            AdminPort = port;
            return this;
        }

        /// <summary>
        /// Sets the remote hos of an already running hoverfly instance.
        /// </summary>
        /// <param name="remoteHost">The remote host URL port.</param>
        /// <returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig SetRemoteHost(Uri remoteHost)
        {
            if (remoteHost == null)
                throw new ArgumentNullException(nameof(remoteHost));

            RemoteHost = remoteHost;
            return this;
        }

        /// <summary>
        /// Sets if localhost should be proxied.
        /// </summary>
        /// <param name="proxyLoalhost">If localhost should be proxied or not.</param>
        /// <returns>Returns <see cref="HoverflyConfig"/> for further customizations.</returns>
        public HoverflyConfig SetProxyLocalhost(bool proxyLoalhost)
        {
            ProxyLocalhost = proxyLoalhost;
            return this;
        }


        /// <summary>
        /// Sets the base path to the hoverfly.exe.
        /// </summary>
        /// <param name="hoverflyBasePath">The base path to the hoverfly.exe.</param>
        /// <returns></returns>
        public HoverflyConfig SetHoverflyBasePath(string hoverflyBasePath)
        {
            if (string.IsNullOrWhiteSpace(hoverflyBasePath))
                throw new ArgumentNullException(nameof(hoverflyBasePath));

            if (hoverflyBasePath.Contains("hoverfly.exe"))
                hoverflyBasePath = hoverflyBasePath.Replace("hoverfly.exe", string.Empty);

            HoverflyBasePath = hoverflyBasePath;
            return this;
        }
    }
}