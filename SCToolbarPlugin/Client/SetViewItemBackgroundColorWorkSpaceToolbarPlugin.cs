﻿using System;
using System.Collections.Generic;
using System.Windows.Media;
using VideoOS.Platform;
using VideoOS.Platform.Client;

namespace SCToolbarPlugin.Client
{
    class SetViewItemBackgroundColorWorkSpaceToolbarPluginInstance : WorkSpaceToolbarPluginInstance
    {
        private SolidColorBrush _color;
        private Item _window;

        public SetViewItemBackgroundColorWorkSpaceToolbarPluginInstance(SolidColorBrush color)
        {
            _color = color;
        }

        public override void Init(Item window)
        {
            _window = window;
            Title = ColorConverter.ConvertColorToCommonName(_color.Color);
        }

        public override void Activate()
        {
            VideoOS.Platform.Messaging.Message message = new VideoOS.Platform.Messaging.Message(SCToolbarPluginDefinition.SetViewItemBackgroundColor);
            message.Data = new SCToolbarPluginDefinition.ColorMessageData() { Color = _color, ViewItemInstanceFQID = null, WindowFQID = _window.FQID };
            EnvironmentManager.Instance.SendMessage(message);
        }

        public override void Close()
        {
        }
    }

    class SetViewItemBackgroundColorWorkSpaceToolbarPlugin : WorkSpaceToolbarPlugin
    {
        private static readonly Guid PluginId = new Guid("EC586601-240C-4433-9F75-65A993113A06");

        private SolidColorBrush _color;

        public SetViewItemBackgroundColorWorkSpaceToolbarPlugin(SolidColorBrush color)
        {
            _color = color;
        }

        public override Guid Id
        {
            get { return PluginId; }
        }

        public override string Name
        {
            get { return "Set view item background name sample work space toolbar plugin"; }
        }

        public override void Init()
        {
            WorkSpaceToolbarPlaceDefinition.WorkSpaceIds = new List<Guid>() { ClientControl.LiveBuildInWorkSpaceId, ClientControl.PlaybackBuildInWorkSpaceId };
            WorkSpaceToolbarPlaceDefinition.WorkSpaceStates = new List<WorkSpaceState>() { WorkSpaceState.Normal };
        }

        public override void Close()
        {
        }

        public override WorkSpaceToolbarPluginInstance GenerateWorkSpaceToolbarPluginInstance()
        {
            return new SetViewItemBackgroundColorWorkSpaceToolbarPluginInstance(_color);
        }
    }
}
