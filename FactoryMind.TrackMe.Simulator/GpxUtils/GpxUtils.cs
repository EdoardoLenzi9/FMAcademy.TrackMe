using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FactoryMind.TrackMe.Simulatore.Models;

namespace FactoryMind.TrackMe.Simulatore.GpxUtils
{
    public sealed class GpxUtility
    {
        public static GpxPoint ReadPointAt(Gpx gpx, int pos)
        {
            try
            {
                var gpxElement = XElement.Load(gpx.SaveLocation);
                var track = gpxElement.Elements().Where(e => e.Name.LocalName == "trk").Single();
                var trackSegment = track.Elements().Where(e => e.Name.LocalName == "trkseg").Single();
                var Point = trackSegment.Elements().Where(e => e.Name.LocalName == "trkpt").ElementAt(pos);
                var lat = float.Parse(Point.Attribute("lat").Value);
                var lon = float.Parse(Point.Attribute("lon").Value);
                return new GpxPoint { Lat = lat, Lon = lon };
            }
            catch (FileNotFoundException)
            {
                throw new Exception($"\n\nErrore .gpx non trovato in {gpx.SaveLocation}\n\n");
            }
            catch (Exception)
            {
                throw new Exception($"\n\nErrore formato .gpx nel file {gpx.SaveLocation}\n\n");
            }
        }

        public static GpxPoint ReadPointAt(string path, int pos)
        {
            try
            {
                var gpxElement = XElement.Load(path);
                var track = gpxElement.Elements().Where(e => e.Name.LocalName == "trk").Single();
                var trackSegment = track.Elements().Where(e => e.Name.LocalName == "trkseg").Single();
                var Point = trackSegment.Elements().Where(e => e.Name.LocalName == "trkpt").ElementAt(pos);
                var lat = float.Parse(Point.Attribute("lat").Value);
                var lon = float.Parse(Point.Attribute("lon").Value);
                return new GpxPoint { Lat = lat, Lon = lon };
            }
            catch (FileNotFoundException)
            {
                throw new Exception($"\n\nErrore .gpx non trovato in {path}\n\n");
            }
            catch (Exception)
            {
                throw new Exception($"\n\nErrore formato .gpx nel file {path}\n\n");
            }
        }
    }
}