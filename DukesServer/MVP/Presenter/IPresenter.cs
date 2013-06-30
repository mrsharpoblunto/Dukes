using System;
using System.Collections.Generic;
using System.Text;
using DukesServer.MVP.View;

namespace DukesServer.MVP.Presenter
{
    internal interface IPresenter<T> where T : IView
    {
        T View { get; }
    }
}