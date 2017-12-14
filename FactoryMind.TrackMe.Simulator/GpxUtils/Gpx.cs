using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace FactoryMind.TrackMe.Simulatore.GpxUtils
{
    public sealed class Gpx
    {
        private string _head = $"<?xml version=\"{"1.0"}\" encoding=\"{"UTF-8"}\" standalone=\"{"no"}\" ?>";
        public XElement GpxElement { get; set; } = new XElement("gpx", new XAttribute("creators", "Dario ed Edoardo"), new XAttribute("version", "1.0"));
        private XElement _metadata = new XElement("metadata");
        private XElement _track = new XElement("trk");
        private XElement _trackName = new XElement("name");
        private XElement _trackSegment = new XElement("trkseg");
        public string SaveLocation = null;

        public Gpx()
        {
            _metadata.Value = DateTime.Now.ToString();
            _trackName.Value = "Auto Generated track";
            GpxElement.Add(_metadata);
            GpxElement.Add(_track);
            _track.Add(_trackName);
            _track.Add(_trackSegment);
        }

        public void CreatePoint(float lat, float lon)
        {
            var trkpt = new XElement("trkpt", new XAttribute("lat", $"{lat}"), new XAttribute("lon", $"{lon}"));
            var name = new XElement("name");
            name.Value = $"TP{_trackSegment.Elements().Count().ToString()}";
            trkpt.Add(name);
            _trackSegment.Add(trkpt);
        }

        public void PrintOnConsole()
        {
            System.Console.WriteLine($"{_head}\n{GpxElement}");
        }

        public void SaveToLocation(string location, string fileName)
        {
            string content = $"{_head}\n{GpxElement}";
            SaveLocation = $"{location}/{fileName}";
            File.WriteAllText(SaveLocation, content);
        }
    }
}