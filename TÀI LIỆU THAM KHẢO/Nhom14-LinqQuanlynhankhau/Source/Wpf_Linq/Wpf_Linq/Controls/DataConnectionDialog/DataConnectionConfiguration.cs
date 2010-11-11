//------------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace Microsoft.Data.ConnectionUI
{
	/// <summary>
	/// Provide a default implementation for the storage of DataConnection Dialog UI configuration.
	/// </summary>
	public class DataConnectionConfiguration
	{        
        // Available data sources:
        private IDictionary<string, DataSource> dataSources;

        // Available data providers: 
        private IDictionary<string, DataProvider> dataProviders;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="path">Configuration file path.</param>
		public DataConnectionConfiguration()
		{
		}

		public void LoadConfiguration(DataConnectionDialog dialog)
		{
			dialog.DataSources.Add(DataSource.SqlDataSource);

			dialog.UnspecifiedDataSource.Providers.Add(DataProvider.SqlDataProvider);

			this.dataSources = new Dictionary<string, DataSource>();
			this.dataSources.Add(DataSource.SqlDataSource.Name, DataSource.SqlDataSource);

			this.dataProviders = new Dictionary<string, DataProvider>();
			this.dataProviders.Add(DataProvider.SqlDataProvider.Name, DataProvider.SqlDataProvider);


			DataSource ds = null;
            string dsName = "MicrosoftSqlServer";
			if (!String.IsNullOrEmpty(dsName) && this.dataSources.TryGetValue(dsName, out ds))
			{
				dialog.SelectedDataSource = ds;
			}

			DataProvider dp = null;
            string dpName = "System.Data.SqlClient";
			if (!String.IsNullOrEmpty(dpName) && this.dataProviders.TryGetValue(dpName, out dp))
			{
				dialog.SelectedDataProvider = dp;
			}
		}
	}
}
