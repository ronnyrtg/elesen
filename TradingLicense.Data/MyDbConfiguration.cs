﻿using System.Data.Entity;

namespace TradingLicense.Data
{
    public class MyDbConfiguration : DbConfiguration
    {
        public MyDbConfiguration()
        {
            this.AddInterceptor(new NVarcharInterceptor()); //add this line to existing config.
        }
    }
}