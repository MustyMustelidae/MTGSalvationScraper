using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace MTGSalvationScraper
{
    public static class HtmlNodeExt
    {
        public static HtmlNode SearchChildNode(this HtmlNode htmlNode, Func<HtmlNode, bool> predicate)
        {
            return SearchChildNodes(htmlNode, predicate).FirstOrDefault();
        }
        public static HtmlNode SearchChildNodeByAttributes(this HtmlNode htmlNode, Func<HtmlAttribute, bool> predicate)
        {
            return SearchChildNodesByAttributes(htmlNode, predicate).FirstOrDefault();
        }
        public static IEnumerable<HtmlNode> SearchChildNodes(this HtmlNode htmlNode, Func<HtmlNode, bool> predicate)
        {
            var nodes = new List<HtmlNode>();
            FindChildNodesByPredicate(htmlNode, predicate,ref nodes);
            return nodes;
        }
        public static IEnumerable<HtmlNode> SearchChildNodesByAttributes(this HtmlNode htmlNode, Func<HtmlAttribute, bool> predicate)
        {
            var nodes = new List<HtmlNode>();
            FindChildNodesByPredicate(htmlNode, node => node.Attributes.Any(predicate), ref nodes);
            return nodes;
        }
        private static void FindChildNodesByPredicate(HtmlNode currentNode, Func<HtmlNode, bool> searchPredicate, ref List<HtmlNode> foundNodes)
        {
            if (!currentNode.HasChildNodes) return;

            foreach (var childNode in currentNode.ChildNodes)
            {
                FindChildNodesByPredicate(childNode, searchPredicate, ref foundNodes);
            }
            if (searchPredicate(currentNode))
            {
                foundNodes.Add(currentNode);
            }
        }
    }
}
