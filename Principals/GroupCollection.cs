using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using SourceCode.Hosting.Server.Interfaces;

namespace K2Community.CSP.CRM.Principals
{
    class GroupCollection : IGroupCollection
    {
        #region Class Level Fields

        #region Private Fields
        /// <summary>
        /// Local list variable used to hold all the items in this collection.
        /// </summary>
        private List<IGroup> list = new List<IGroup>();
        #endregion

        #endregion

        #region Indexers

        #region IGroup this[int index]
        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        public IGroup this[int index]
        {
            get
            {
                // Get the element at the specified index.
                return list[index];
            }
        }
        #endregion

        #endregion

        #region Default Constructor
        /// <summary>
        /// Instantiates a new GroupCollection.
        /// </summary>
        public GroupCollection()
        {
            // No implementation necessary.
        }
        #endregion

        #region Methods

        #region void Add(IGroup group)
        /// <summary>
        /// Adds an IGroup to the collection.
        /// </summary>
        /// <param name="group">An IGroup representing the group to add to the collection.</param>
        public void Add(IGroup group)
        {
            // Add the group to the collection.
            list.Add(group);
        }
        #endregion

        #region IEnumerator GetEnumerator()
        /// <summary>
        /// Returns an enumerator that iterates through the GroupCollection.
        /// </summary>
        /// <returns>An IEnumerator for the GroupCollection.</returns>
        public IEnumerator GetEnumerator()
        {
            // Get an enumerator.
            return list.GetEnumerator();
        }
        #endregion

        #endregion
    }
}