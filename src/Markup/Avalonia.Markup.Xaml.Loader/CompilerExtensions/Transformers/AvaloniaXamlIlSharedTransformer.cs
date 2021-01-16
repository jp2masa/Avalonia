using System;
using System.Collections.Generic;
using System.Linq;

using XamlX;
using XamlX.Ast;
using XamlX.Transform;
using XamlX.TypeSystem;

namespace Avalonia.Markup.Xaml.XamlIl.CompilerExtensions.Transformers
{
    class AvaloniaXamlIlSharedTransformer : IXamlAstTransformer
    {
        public IXamlAstNode Transform(AstTransformationContext context, IXamlAstNode node)
        {
            if (node is XamlValueWithManipulationNode nm
                && context.GetAvaloniaTypes().IResourceDictionary.IsDirectlyAssignableFrom(nm.Type.GetClrType())
                && nm.Manipulation is XamlObjectInitializationNode ni
                && ni.Manipulation is XamlManipulationGroupNode ng)
            {
                TransformInner(context, ng);
            }

            return node;
        }

        private static IXamlAstNode TransformInner(AstTransformationContext context, XamlManipulationGroupNode n)
        {
            foreach (var man in n.Children)
            {
                if (man is XamlManipulationGroupNode ng)
                {
                    TransformInner(context, ng);
                }

                if (!(man is XamlPropertyAssignmentNode np)
                    || !(np.Values.Count > 1)
                    || !(np.Values[1] is XamlValueWithManipulationNode nm))
                {
                    continue;
                }

                var isSharedResult = IsShared(nm.Manipulation);
                
                if (isSharedResult is bool isShared)
                {
                    if (!(nm.Manipulation is XamlManipulationGroupNode))
                    {
                        np.Values[1] = nm.Value;
                    }

                    if (!isShared)
                    {
                        np.Values[1] = new XamlAstNewClrObjectNode(
                            np,
                            new XamlAstClrTypeReference(np, context.GetAvaloniaTypes().ResourceFactory, false),
                            context.GetAvaloniaTypes().ResourceFactory.Constructors.Single(),
                            new List<IXamlAstValueNode>() { new XamlDeferredContentNode(np.Values[1], context.Configuration) });
                    }
                }
            }

            return n;
        }

        private static bool? IsShared(IXamlAstManipulationNode n)
        {
            if (n is XamlObjectInitializationNode ni)
            {
                return IsShared(ni.Manipulation);
            }

            if (n is XamlManipulationGroupNode ng)
            {
                foreach (var man in ng.Children)
                {
                    var isShared = IsShared(man);

                    if (isShared.HasValue)
                    {
                        if (!(man is XamlManipulationGroupNode))
                        {
                            ng.Children.Remove(man);
                        }

                        return isShared.Value;
                    }
                }

                return null;
            }

            if (n is XamlAstXmlDirective d
                && d.Namespace == XamlNamespaces.Xaml2006
                && d.Name == "Shared")
            {
                if (d.Values.Count != 1)
                {
                    throw new XamlParseException("x:Shared should have a single value!", d);
                }

                var text = (XamlAstTextNode)d.Values[0];

                if (!Boolean.TryParse(text.Text, out var isShared))
                {
                    throw new XamlParseException("x:Shared value should be a boolean!", d);
                }

                return isShared;
            }

            return null;
        }
    }
}
