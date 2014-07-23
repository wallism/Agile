using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agile.Framework
{
    /// <summary>
    /// Classes that represent 'Junction' tables in the database implement this framework interface
    /// </summary>
    public interface IJunctionClass
    {
//        bool CheckValue(TP parent);
//        bool CheckValue(TC child);
//        void SetValue(TP parent);
//        void SetValue(TC child);
    }

    /// <summary>
    /// Extensions for reconcilation of data (lists)
    /// </summary>
    public static class ReconcileExtensions
    {


//        public static List<TJ> GetMatches<TJ, TC, TP>(this IEnumerable<TJ> junctionItems
//            , IEnumerable<TC> childItems
//            , TP parent)
//            where TJ : IJunctionClass<TP, TC>
//        {
//            return (from junctionItem in junctionItems
//                    from child in childItems
//                    where junctionItem.CheckValue(child)
//                    select junctionItem).ToList();
//        }
//
//        public static List<TJ> GetRemoveItems<TJ, TC, TP>(this IEnumerable<TJ> junctionItems
//            , IEnumerable<TC> childItems
//            , TP parent)
//            where TJ : IJunctionClass<TP, TC>
//        {
//            return (from junctionItem in junctionItems
//                    where !(junctionItems.GetMatches(childItems, parent).Contains(junctionItem))
//                    select junctionItem).ToList();
//        }
//
//        public static List<TJ> GetAddItems<TJ, TC, TP>(this IEnumerable<TJ> junctionItems
//            , IEnumerable<TC> childItems
//            , TP parent)
//            where TJ : IJunctionClass<TP, TC>, new()
//        {
//            var noMatch = childItems.Where(childItem => !HasMatch(junctionItems, childItem, parent)).ToList();
//            var newList = new List<TJ>();
//            noMatch.ForEach(item =>
//                                {
//                                    var junction = new TJ();
//                                    junction.SetValue(item);
//                                    junction.SetValue(parent);
//                                    newList.Add(junction);
//                                });
//            return newList;
//        }
//
//        private static bool HasMatch<TJ, TC, TP>(IEnumerable<TJ> junctionItems
//            , TC child, TP parent) 
//            where TJ : IJunctionClass<TP, TC>
//        {
//            return junctionItems.Any(junctionItem => junctionItem.CheckValue(child));
//        }

        public static List<TJ> GetRemoveItems<TJ, TC>(this IEnumerable<TJ> junctionItems
            , IEnumerable<TC> childItems
            , Func<TJ, TC, bool> match)
        {
            return (from junctionItem in junctionItems
                    where !GetExistingJunctionItems(junctionItems, childItems, match).Contains(junctionItem)
                    select junctionItem).ToList();
        }

        private static List<TJ> GetExistingJunctionItems<TJ, TC>(this IEnumerable<TJ> junctionItems
            , IEnumerable<TC> childItems
            , Func<TJ, TC, bool> match)
        {
            return (from junctionItem in junctionItems
                    from child in childItems
                    where match(junctionItem, child)
                    select junctionItem).ToList();
        }

        public static List<TC> GetAddItems<TJ, TC>(this IEnumerable<TJ> junctionItems
            , IEnumerable<TC> childItems
            , Func<TJ, TC, bool> match)
        {
            return childItems.Where(childItem => 
                !HasMatch(junctionItems, childItem, match)).ToList();
        }

        public static List<TC> GetExistingItems<TJ, TC>(this IEnumerable<TJ> junctionItems
            , IEnumerable<TC> childItems
            , Func<TJ, TC, bool> match)
        {
            return childItems.Where(childItem =>
                HasMatch(junctionItems, childItem, match)).ToList();
        }

        private static bool HasMatch<TJ, TC>(IEnumerable<TJ> junctionItems, TC child, Func<TJ, TC, bool> match)
        {
            return junctionItems.Any(junctionItem 
                => match(junctionItem, child));
        }
    }

    public static class JunctionHelper
    {
    }

}
