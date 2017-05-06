using MTAConvert.Enum;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace MTAConvert.Converter
{
    public class MapConverter
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public async Task<string> ConvertMap(string mapSource, ConvertType convertType, VehicleSpawnType vehicleSpawnType, bool convertVehicles, bool convertObjects, bool convertRemovedObjects, float streamDistance, float drawDistance, bool addComments)
        {
            try
            {
                string resultString = string.Empty;
                using (XmlReader reader = XmlReader.Create(new StringReader(mapSource), new XmlReaderSettings() { Async = true }))
                {
                    while (await reader.ReadAsync())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.LocalName.StartsWith("<map")) continue;

                            if (reader.LocalName == "object")
                            {
                                if (!convertObjects) break;
                                switch (convertType)
                                {
                                    case ConvertType.Default:
                                    {
                                        if (addComments) resultString += $"CreateObject({reader.GetAttribute("model")}, {reader.GetAttribute("posX")}, {reader.GetAttribute("posY")}, {reader.GetAttribute("posZ")}, {reader.GetAttribute("rotX")}, {reader.GetAttribute("rotY")}, {reader.GetAttribute("rotZ")}, {drawDistance}); // {reader.GetAttribute("id")} {Environment.NewLine}";
                                        else resultString += $"CreateObject({reader.GetAttribute("model")}, {reader.GetAttribute("posX")}, {reader.GetAttribute("posY")}, {reader.GetAttribute("posZ")}, {reader.GetAttribute("rotX")}, {reader.GetAttribute("rotY")}, {reader.GetAttribute("rotZ")}, {drawDistance}); {Environment.NewLine}";
                                        break;
                                    }
                                    case ConvertType.Streamer:
                                    {
                                        if (addComments) resultString += $"CreateDynamicObject({reader.GetAttribute("model")}, {reader.GetAttribute("posX")}, {reader.GetAttribute("posY")}, {reader.GetAttribute("posZ")}, {reader.GetAttribute("rotX")}, {reader.GetAttribute("rotY")}, {reader.GetAttribute("rotZ")}, -1, -1, -1, {streamDistance}, {drawDistance}); // {reader.GetAttribute("id")} {Environment.NewLine}";
                                        else resultString += $"CreateDynamicObject({reader.GetAttribute("model")}, {reader.GetAttribute("posX")}, {reader.GetAttribute("posY")}, {reader.GetAttribute("posZ")}, {reader.GetAttribute("rotX")}, {reader.GetAttribute("rotY")}, {reader.GetAttribute("rotZ")}, -1, -1, -1, {streamDistance}, {drawDistance}); {Environment.NewLine}";
                                        break;
                                    }
                                }
                            }

                            if (reader.LocalName == "vehicle")
                            {
                                if (!convertVehicles) break;
                                switch (vehicleSpawnType)
                                {
                                    case VehicleSpawnType.AddStaticVehicle:
                                    {
                                        if (addComments) resultString += $"AddStaticVehicle({reader.GetAttribute("model")}, {reader.GetAttribute("posX")}, {reader.GetAttribute("posY")}, {reader.GetAttribute("posZ")}, {reader.GetAttribute("rotZ")}, -1, -1); // {reader.GetAttribute("id")} {Environment.NewLine}";
                                        else resultString += $"AddStaticVehicle({reader.GetAttribute("model")}, {reader.GetAttribute("posX")}, {reader.GetAttribute("posY")}, {reader.GetAttribute("posZ")}, {reader.GetAttribute("rotZ")}, -1, -1); {Environment.NewLine}";
                                        break;
                                    }
                                    case VehicleSpawnType.AddStaticVehicleEx:
                                    {
                                        if (addComments) resultString += $"AddStaticVehicle({reader.GetAttribute("model")}, {reader.GetAttribute("posX")}, {reader.GetAttribute("posY")}, {reader.GetAttribute("posZ")}, {reader.GetAttribute("rotZ")}, -1, -1, 60); // {reader.GetAttribute("id")} {Environment.NewLine}";
                                        else resultString += $"AddStaticVehicle({reader.GetAttribute("model")}, {reader.GetAttribute("posX")}, {reader.GetAttribute("posY")}, {reader.GetAttribute("posZ")}, {reader.GetAttribute("rotZ")}, -1, -1, 60); {Environment.NewLine}";
                                        break;
                                    }
                                    case VehicleSpawnType.CreateVehicle:
                                    {
                                        if (addComments) resultString += $"CreateVehicle({reader.GetAttribute("model")}, {reader.GetAttribute("posX")}, {reader.GetAttribute("posY")}, {reader.GetAttribute("posZ")}, {reader.GetAttribute("rotZ")}, -1, -1, 60); // {reader.GetAttribute("id")} {Environment.NewLine}";
                                        else resultString += $"CreateVehicle({reader.GetAttribute("model")}, {reader.GetAttribute("posX")}, {reader.GetAttribute("posY")}, {reader.GetAttribute("posZ")}, {reader.GetAttribute("rotZ")}, -1, -1, 60); {Environment.NewLine}";
                                        break;
                                    }
                                }
                            }

                            if (reader.LocalName == "removeWorldObject")
                            {
                                if (!convertRemovedObjects) break;
                                if (addComments) resultString += $"RemoveBuildingForPlayer(playerid, {reader.GetAttribute("model")}, {reader.GetAttribute("posX")}, {reader.GetAttribute("posY")}, {reader.GetAttribute("posZ")}, {reader.GetAttribute("radius")}); // {reader.GetAttribute("id")} {Environment.NewLine}";
                                else resultString += $"RemoveBuildingForPlayer(playerid, {reader.GetAttribute("model")}, {reader.GetAttribute("posX")}, {reader.GetAttribute("posY")}, {reader.GetAttribute("posZ")}, {reader.GetAttribute("radius")}); {Environment.NewLine}";
                            }
                        }
                    }
                }
                return resultString;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured during conversion: {ex.Message}.");
                _logger.Error($"{ex.Source} {ex.StackTrace}");
                return ex.Message;
            }
            finally
            {
                _logger.Info("Conversion finished.");
            }
        }
    }
}