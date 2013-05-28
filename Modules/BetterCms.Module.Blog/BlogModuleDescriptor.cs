﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;

using Autofac;

using BetterCms.Api;
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Blog.Accessors;
using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Helpers.Extensions;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Registration;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Pages.Accessors;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Api.Events;

namespace BetterCms.Module.Blog
{
    /// <summary>
    /// Blog module descriptor
    /// </summary>
    public class BlogModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "blog";

        internal const string BlogAreaName = "bcms-blog";

        /// <summary>
        /// The blog java script module descriptor
        /// </summary>
        private readonly BlogJsModuleIncludeDescriptor blogJsModuleIncludeDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogModuleDescriptor" /> class.
        /// </summary>
        public BlogModuleDescriptor(ICmsConfiguration configuration) : base(configuration)
        {
            blogJsModuleIncludeDescriptor = new BlogJsModuleIncludeDescriptor(this);

            RootApiContext.Events.PageRetrieved += Events_PageRetrieved;
        }

        /// <summary>
        /// Gets the module name.
        /// </summary>
        /// <value>
        /// The module name.
        /// </value>
        public override string Name
        {
            get
            {
                return ModuleName;
            }
        }

        /// <summary>
        /// Gets the module description.
        /// </summary>
        /// <value>
        /// The module description.
        /// </value>
        public override string Description
        {
            get
            {
                return "Blog module for BetterCMS.";
            }
        }

        /// <summary>
        /// Gets the name of the module area.
        /// </summary>
        /// <value>
        /// The name of the module area.
        /// </value>
        public override string AreaName
        {
            get
            {
                return BlogAreaName;
            }
        }

        /// <summary>
        /// Registers the sidebar main projections.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns></returns>
        public override IEnumerable<IPageActionProjection> RegisterSidebarMainProjections(ContainerBuilder containerBuilder)
        {
            return new IPageActionProjection[]
                {
                    new ButtonActionProjection(blogJsModuleIncludeDescriptor, page => "postNewArticle")
                        {
                            Title = () => BlogGlobalization.Sidebar_AddNewPostButtonTitle,
                            Order = 200,
                            CssClass = page => "bcms-sidemenu-btn bcms-btn-blog-add",
                            AccessRole = RootModuleConstants.UserRoles.EditContent
                        }
                };
        }

        /// <summary>
        /// Registers java script modules.
        /// </summary>        
        /// <returns>
        /// Enumerator of known JS modules list.
        /// </returns>
        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return new[]
                {
                    blogJsModuleIncludeDescriptor
                };
        }

        /// <summary>
        /// Registers the style sheet files.
        /// </summary>        
        /// <returns>Enumerator of known module style sheet files.</returns>
        public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return new[]
                {
                    new CssIncludeDescriptor(this, "bcms.blog.css")
                };
        }

        /// <summary>
        /// Registers the site settings projections.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns>List of page action projections.</returns>
        public override IEnumerable<IPageActionProjection> RegisterSiteSettingsProjections(ContainerBuilder containerBuilder)
        {
            return new IPageActionProjection[]
                {
                    new LinkActionProjection(blogJsModuleIncludeDescriptor, page => "loadSiteSettingsBlogs")
                        {
                            Order = 1200,
                            Title = () => BlogGlobalization.SiteSettings_BlogsMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.MultipleRoles(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent, RootModuleConstants.UserRoles.DeleteContent)
                        }                                      
                };
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            RegisterContentRendererType<BlogPostContentAccessor, BlogPostContent>(containerBuilder);
            RegisterStylesheetRendererType<PageStylesheetAccessor, BlogPost>(containerBuilder);
            RegisterJavaScriptRendererType<PageJavaScriptAccessor, BlogPost>(containerBuilder);

            containerBuilder.RegisterType<DefaultOptionService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultAuthorService>().AsImplementedInterfaces().InstancePerLifetimeScope(); 
            containerBuilder.RegisterType<DefaultBlogService>().AsImplementedInterfaces().InstancePerLifetimeScope();          
        }

        /// <summary>
        /// Occurs, when the page is retrieved.
        /// </summary>
        /// <param name="args">The <see cref="PageRetrievedEventArgs" /> instance containing the event data.</param>
        private void Events_PageRetrieved(PageRetrievedEventArgs args)
        {
            if (args != null && args.RenderPageData != null)
            {
                args.RenderPageData.ExtendWithBlogData(args.PageData);
                if (args.RenderPageData.IsBlogPost() && !args.RenderPageData.IsBlogPostActive() && !args.RenderPageData.CanManageContent)
                {
                    args.RenderPageData.ActionResult = new HttpNotFoundResult();
                }
            }
        }
    }
}
