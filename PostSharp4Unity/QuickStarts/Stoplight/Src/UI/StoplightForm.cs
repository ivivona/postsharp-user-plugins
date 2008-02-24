﻿//===============================================================================
// Microsoft patterns & practices
// Unity Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using PostSharp4Unity;
using StopLight.ServiceImplementations;
using StopLight.ServiceInterfaces;

namespace StopLight.UI
{
    [Configurable]
	public partial class StoplightForm : Form, IStoplightView
	{
		private StoplightPresenter presenter;

		public StoplightForm()
		{
    		InitializeComponent();

            presenter.SetView(this);
		}

		[Dependency]
		public StoplightPresenter Presenter
		{
			get { return presenter; }
			set
			{
				presenter = value;
			}
		}

		#region IStoplightView Members

		public Color CurrentColor
		{
			get { return stopLightPanel.BackColor; }
			set
			{
				stopLightPanel.BackColor = value;
				RaisePropertyChanged(StoplightViewProperties.CurrentColor);
			}
		}

		public string GreenDuration
		{
			get { return greenDurationTextBox.Text; }
			set
			{
				greenDurationTextBox.Text = value;
				RaisePropertyChanged(StoplightViewProperties.GreenDuration);
			}
		}

		public string YellowDuration
		{
			get { return yellowDurationTextBox.Text; }
			set
			{
				yellowDurationTextBox.Text = value;
				RaisePropertyChanged(StoplightViewProperties.YellowDuration);
			}
		}

		public string RedDuration
		{
			get { return redDurationTextBox.Text; }
			set
			{
				redDurationTextBox.Text = value;
				RaisePropertyChanged(StoplightViewProperties.RedDuration);
			}
		}

		public event EventHandler UpdateClicked;

		public event EventHandler ForceChangeClicked;

		public void SetError(string propertyName, string errorMessage)
		{
			Dictionary<string, Control> controlsByName = new Dictionary<string, Control>();
			controlsByName.Add(StoplightViewProperties.GreenDuration, greenDurationTextBox);
			controlsByName.Add(StoplightViewProperties.YellowDuration, yellowDurationTextBox);
			controlsByName.Add(StoplightViewProperties.RedDuration, redDurationTextBox);


			if(controlsByName.ContainsKey(propertyName))
			{
				errorProvider.SetError(controlsByName[propertyName], errorMessage);
			}
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		// Event firing helpers

		protected virtual void RaisePropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handlers = PropertyChanged;
			if(handlers != null)
			{
				handlers(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected virtual void RaiseUpdateClicked()
		{
			EventHandler handlers = UpdateClicked;
			if(handlers != null)
			{
				handlers(this, EventArgs.Empty);
			}
		}

		protected virtual void RaiseForceChangeClicked()
		{
			EventHandler handlers = ForceChangeClicked;
			if(handlers != null)
			{
				handlers(this, EventArgs.Empty);
			}
		}

		private void OnUpdateScheduleClicked(object sender, EventArgs e)
		{
			RaiseUpdateClicked();
		}

		private void OnChangeLightClicked(object sender, EventArgs e)
		{
			RaiseForceChangeClicked();
		}
	}
}
