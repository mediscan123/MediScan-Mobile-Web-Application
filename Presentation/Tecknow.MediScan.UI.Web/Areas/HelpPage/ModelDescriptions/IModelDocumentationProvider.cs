using System;
using System.Reflection;

namespace Tecknow.MediScan.UI.Web.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);
        string GetDocumentation(Type type);
    }
}