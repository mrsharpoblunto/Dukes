using System;
using System.Collections.Generic;
using System.Text;
using DukesServer.MVP.View;

namespace DukesServer.MVP.Presenter
{
    internal class PresenterBase<T> : IPresenter<T> where T : IView
    {
        private readonly T _view;

        public T View
        {
            get { return _view; }
        }

        public PresenterBase(T view)
        {
            _view = view;
        }
    }
}