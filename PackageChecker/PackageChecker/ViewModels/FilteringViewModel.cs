using PackageChecker.BindingBase;
using PackageChecker.Filtering;
using PackageChecker.Models;
using PackageChecker.WindowManagement;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PackageChecker.ViewModels
{
	internal class FilteringViewModel : BindableBase
	{
		private readonly FilteringModel _model = new FilteringModel();

		internal FilteringViewModel()
		{
			_model.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);

			AddFilter = new BindCommand(param =>
			{
				try
				{
					_model.AddExpression(param == null ? CurrentFilteringExpression : (string)param);
					_model.CurrentFilteringExpression = string.Empty;
				}
				catch (ArgumentException e)
				{
					WindowHelper.ShowError(e.Message);
				}
			});

			EditFilter = new BindCommand(param =>
			{
				try
				{
					_model.EditExpression(SelectedIndex);
				}
				catch (ArgumentException e)
				{
					WindowHelper.ShowError(e.Message);
				}
			});

			RemoveFilter = new BindCommand(param =>
			{
				try
				{
					_model.RemoveExpression(SelectedIndex);
				}
				catch (ArgumentException e)
				{
					WindowHelper.ShowError(e.Message);
				}
			});

			ShowInfo = new BindCommand(param =>
			{
				string helpMessage = _model.GetHelpMessage();
				WindowHelper.ShowInfo(helpMessage);
			});
		}

		public ICommand AddFilter { get; }
		public ICommand EditFilter { get; }
		public ICommand RemoveFilter { get; }
		public ICommand ShowInfo { get; }

		public ReadOnlyObservableCollection<string> FilteringExpressions => _model.FilteringExpressions;
		public int SelectedIndex { get { return _model.SelectedIndex; } set { _model.SelectedIndex = value; } }
		public string CurrentFilteringExpression { get { return _model.CurrentFilteringExpression; } set { _model.CurrentFilteringExpression = value; } }
		public string CurrentFilteringExpressionHint => _model.CurrentFilteringExpressionHint;

		internal IFilteringManager GetFilteringManager()
		{
			return _model;
		}

		internal FilteringModel.FilteringState GetState()
		{
			return FilteringModel.FilteringState.GetState(_model);
		}

		internal void SetState(FilteringModel.FilteringState state)
		{
			state.SetState(_model);
		}
	}
}
