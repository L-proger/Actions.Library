using Actions.Nuget.Nuspec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Actions.Nuget {




    public class NuspecFile {
        public Nuspec.Package package;

        public NuspecFile() {
            package = new Nuspec.Package();
        }
        public NuspecFile(string id, string version, string authors, string owners, Uri projectUrl, bool requireLicenseAcceptance, string description, string[] tags, string license) {
            package = new Nuspec.Package();
            package.Metadata = new Nuspec.PackageMetadata();
            package.Metadata.Authors = authors;
            package.Metadata.Owners = owners;
            package.Metadata.Id = id;
            package.Metadata.Version = version;
            package.Metadata.ProjectUrl = projectUrl.ToString();
            package.Metadata.RequireLicenseAcceptance = requireLicenseAcceptance;
            package.Metadata.Description = description;
            package.Metadata.Tags = string.Join(" ", tags);
            package.Metadata.License = new PackageMetadataLicense();
            package.Metadata.License.Value = license;
            package.Metadata.License.Type = "expression";
        }

        public void Serialize(Stream outStream) {
            XmlSerializer serializer = new XmlSerializer(typeof(Nuspec.Package), "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");
            serializer.Serialize(outStream, package);
        }

        public void SerializeToFile(string path) {
            var outFileStream = System.IO.File.Open(path, FileMode.Create, FileAccess.Write);
            Serialize(outFileStream);
            outFileStream.Close();
        }

        public void Deserialize(Stream inStream) {
            XmlSerializer serializer = new XmlSerializer(typeof(Nuspec.Package), "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");
            package = (Nuspec.Package)serializer.Deserialize(inStream);
        }

        public void DeserializeFromFile(string path) {
            var inFileStream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read);
            Deserialize(inFileStream);
            inFileStream.Close();
        }
    }
}
