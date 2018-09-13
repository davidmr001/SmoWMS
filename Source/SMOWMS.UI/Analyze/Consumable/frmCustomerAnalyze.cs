﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smobiler.Core;
using Smobiler.Core.Controls;
using System.Data;

namespace SMOWMS.UI.Analyze.Consumable
{
    partial class frmCustomerAnalyze : Smobiler.Core.Controls.MobileForm
    {
        public frmCustomerAnalyze() : base()
        {
            //This call is required by the SmobilerForm.
            InitializeComponent();
        }
        #region 变量
        private AutofacConfig autofacConfig = new AutofacConfig();//调用配置类
        #endregion
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCustomerAnalyze_Load(object sender, EventArgs e)
        {
            dpStart.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day);
            dpEnd.Value = DateTime.Now;
            dpStart.MaxDate = DateTime.Now;
            dpEnd.MaxDate = DateTime.Now;

            Bind();
        }
        /// <summary>
        /// 数据加载
        /// </summary>
        private void Bind()
        {
            if (dpEnd.Value.Date != DateTime.Now.Date)
            {
                dpEnd.Value = dpEnd.Value.AddHours(23).AddMinutes(59);
            }
            Dictionary<string, Dictionary<string, decimal>> result = autofacConfig.
                ConSalesOrderService.GetCustomerAnalyze(dpStart.Value, dpEnd.Value);
            DataTable table = new DataTable();
            table.Columns.Add("CUSTOMER");      //客户名称
            table.Columns.Add("NAME");        //耗材名称
            table.Columns.Add("QUANTITY");    //耗材库存
            foreach (string CUSTOMER in result.Keys)
            {
                foreach (string NAME in result[CUSTOMER].Keys)
                {
                    table.Rows.Add(CUSTOMER, NAME, result[CUSTOMER][NAME]);
                }
            }
            bcQuant.DataSource = table;
            bcQuant.DataBind();
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dpStart_ValueChanged(object sender, EventArgs e)
        {
            dpEnd.MinDate = dpStart.Value;
            Bind();
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dpEnd_ValueChanged(object sender, EventArgs e)
        {
            dpStart.MaxDate = dpEnd.Value;
            Bind();
        }
    }
}