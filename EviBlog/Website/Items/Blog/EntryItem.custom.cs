﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Web;
using Sitecore.Links;
using Sitecore.Modules.Blog.Managers;
using Sitecore.Security.Accounts;

namespace Sitecore.Modules.Blog.Items.Blog
{
    public partial class EntryItem
    {
        public string[] TagsSplit
        {
            get
            {
                return ExtractTags(false);
            }
            set
            {
                this.Tags.Field.Value = FormTags(value);
            }
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url
        {
            get
            {
                return "http://" + WebUtil.GetHostName() + LinkManager.GetItemUrl(InnerItem);
            }
        }

        /// <summary>
        /// Gets the count of comments for this blog entry.
        /// </summary>
        public int CommentCount
        {
            get
            {
                return CommentManager.GetCommentsCount(this.ID);
            }
        }

        /// <summary>
        /// Gets the creation date of the blog entry item.
        /// </summary>
        public DateTime Created
        {
            get
            {
                return InnerItem.Statistics.Created;
            }
        }

        public User CreatedBy
        {
            get
            {
                //go direct to field since ItemStatistics ignores standard values
                string createdBy = InnerItem[Sitecore.FieldIDs.CreatedBy];
                if (!string.IsNullOrEmpty(createdBy))
                {
                    return User.FromName(createdBy, AccountType.User) as User;
                }
                return null;
            }
        }

        /// <summary>
        /// Forms the tags.
        /// </summary>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        static string FormTags(string[] tags)
        {
            string tagsString = string.Empty;
            for (int i = 0; i < tags.Length; i++)
            {
                tagsString += tags[i].Trim();
                tagsString += i < (tags.Length - 1) ? "," : string.Empty;
            }
            return tagsString;
        }

        /// <summary>
        /// Extracts the tags.
        /// </summary>
        /// <param name="isEncode">if set to <c>true</c> [is encode].</param>
        /// <returns></returns>
        string[] ExtractTags(bool isEncode)
        {
            string[] tags = InnerItem["Tags"].Trim().Split(',');
            System.Collections.ArrayList result = new System.Collections.ArrayList();
            for (int i = 0; i < tags.Length; i++)
            {
                if (tags[i].Trim() != string.Empty)
                {
                    if (isEncode)
                    {
                        result.Add(System.Web.HttpUtility.HtmlEncode(tags[i]));
                    }
                    else
                    {
                        result.Add(tags[i].Trim());
                    }
                }
            }
            return (string[])result.ToArray(typeof(string));
        }
    }
}