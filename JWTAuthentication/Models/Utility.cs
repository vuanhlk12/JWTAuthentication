using JWTAuthentication.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthentication.Authentication
{
    public static class Utility
    {

        public static CategoryModel BuildTree(this List<CategoryModel> nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException("nodes");
            }
            return new CategoryModel().BuildTree(nodes);
        }
        private static CategoryModel BuildTree(this CategoryModel root, List<CategoryModel> nodes)
        {
            if (nodes.Count == 0)
            {
                List<ProductModel> products = new ProductController().GetProductByCategoryID1(root.Id);
                root.ProductsList.AddRange(products);
                return root;
            }

            if (root.Id == null && nodes[0].ParentID != null)
            {
                root.Id = nodes[0].ParentID;
            }

            var children = root.FetchChildren(nodes).ToList();
            root.ChildList.AddRange(children);
            root.RemoveChildren(nodes);

            for (int i = 0; i < children.Count; i++)
            {
                children[i] = children[i].BuildTree(nodes);
                //if (nodes.Count == 0) { break; }
            }

            return root;
        }

        public static IEnumerable<CategoryModel> FetchChildren(this CategoryModel root, List<CategoryModel> nodes)
        {
            return nodes.Where(n => n.ParentID == root.Id);
        }

        public static void RemoveChildren(this CategoryModel root, List<CategoryModel> nodes)
        {
            foreach (var node in root.ChildList)
            {
                nodes.Remove(node);
            }
        }
    }
}
