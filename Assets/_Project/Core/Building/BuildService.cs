using System.Collections.Generic;
using SpaceChaser.Core.Registry;
using UnityEngine;

namespace SpaceChaser.Core.Building
{
    public class BuildService : IBuildService
    {
        private readonly IBuildFactory _factory;
        private readonly IStrutFactory _strutFactory;
        private readonly IFoundationFactory _foundationFactory;
        private readonly BuildPreview _preview;
        public BuildService(IBuildFactory factory, IStrutFactory strutFactory, IFoundationFactory foundationFactory, BuildPreview preview)
        {
            _factory = factory;
            _strutFactory = strutFactory;
            _foundationFactory = foundationFactory;
            _preview = preview;
        }

        public bool Build(BuildData data, Vector2 pos, float rotation)
        {
            _factory.Create(data, pos, rotation);

            return true;
        }
        public bool BuildStrut(StrutData data, Vector2 pos, float rotation)
        {
            IReadOnlyList<Build> builds = _preview.GetAllBuildContacts();

            Strut strut = _strutFactory.Create(data, pos, rotation);

            Physics2D.SyncTransforms();

            foreach (var build in builds)
            {
                build.ConnectTo(strut);
            }

            return true;
        }

        public bool BuildFoundation(FoundationData data, Vector2 pos, float rotation)
        {
            IReadOnlyList<Foundation> foundations = _preview.GetAllFoundationContacts();

            var newFoundation = _foundationFactory.Create(data, pos, rotation);

            foreach (var foundation in foundations)
            {
                newFoundation.RegisterContact(foundation);
                foundation.RegisterContact(newFoundation);
            }

            return true;
        }

        public bool RemoveStrut(Strut strut)
        {
            List<Build> builds = new(strut.GetAttached());

            foreach (Build build in builds)
            {
                build.DisconnectFrom(strut);
            }

            strut.Remove();

            return true;
        }

        public bool RemoveBuild(Build build)
        {
            var struts = build.GetAllStruts();
            build.Remove();

            foreach (var strut in struts)
            {
                if (strut.GetAttached().Count <= 1)
                    RemoveStrut(strut);              // return value ignored ?
            }

            return true;
        }

        public bool RemoveFoundation(Foundation foundation)
        {
            foundation.Remove();

            return true;
        }

        public bool RemoveResource(Resource resource)
        {
            resource.Remove();

            return true;
        }
    }
}