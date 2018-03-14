using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF1
{
    class SetupContent
    {
        public  const string setupcontent1 = "Format type:keyword version: 1.1.0\r\nbegin_<project> ";
        public  const string setupcontent2 = "FirstAvailableStudyAreaNumber 1\r\nFirstAvailableCommSystemNumber 0\r\nFirstAvailableFilterNumber 0\r\nbegin_<requests>\r\ncartesian\r\nsealevel\r\nlocal\r\nCalculationMode New\r\nCEF no\r\nDelaySpread no\r\nDirectionOfArrival no\r\nDirectionOfDeparture no\r\nElectricFieldVsFrequency no\r\nElectricFieldVsTime no\r\nFieldAnimation no\r\nMeanDirectionOfArrival no\r\nMeanDirectionOfDeparture no\r\nPaths no\r\nMeanTimeOfArrival no\r\nEField no\r\nEFieldTotal no\r\nFSPathloss no\r\nFSPower no\r\nPower no\r\nPathloss no\r\nXPathloss no\r\nInputData no\r\nTimeOfArrival no\r\nComplexImpulseResponse no\r\nPowerDelayProfile no\r\nTerrainProfile no\r\nPoyntingVector no\r\nC2I no\r\nStrongestTx no\r\nMaxRenderedPaths 25\r\nFieldAnimationIncrement 10\r\nFieldAnimationTimeAveraged yes\r\nend_<requests>\r\nbegin_<Scales>\r\nbegin_<NPaths>\r\nAutoScaling 1\r\nDrawValues 0\r\nAutoUpdating 1\r\nDiscrete 0\r\nUseGlobalOpacity 1\r\nManualValuesSet 0\r\nClampedHigh 1\r\nClampedLow 1\r\nAlpha 1.000000e+000\r\nManualMin 0.000000e+000\r\nManualMax 1.000000e+000\r\nTextColor 1.000000 1.000000 1.000000\r\nColors 6\r\n0.300000 0.000000 0.500000\r\n0.000000 0.000000 1.000000\r\n0.000000 1.000000 0.000000\r\n1.000000 1.000000 0.000000\r\n1.000000 0.500000 0.000000\r\n1.000000 0.000000 0.000000\r\nPartitionValues\r\n0\r\nend_<NPaths>\r\nbegin_<BER>\r\nAutoScaling 1\r\nDrawValues 0\r\nAutoUpdating 1\r\nDiscrete 0\r\nUseGlobalOpacity 1\r\nManualValuesSet 0\r\nClampedHigh 1\r\nClampedLow 1\r\nAlpha 1.000000e+000\r\nManualMin 0.000000e+000\r\nManualMax 1.000000e+000\r\nTextColor 1.000000 1.000000 1.000000\r\nColors 6\r\n0.300000 0.000000 0.500000\r\n0.000000 0.000000 1.000000\r\n0.000000 1.000000 0.000000\r\n1.000000 1.000000 0.000000\r\n1.000000 0.500000 0.000000\r\n1.000000 0.000000 0.000000\r\nPartitionValues\r\n0\r\nend_<BER>\r\nbegin_<Throughput>\r\nAutoScaling 1\r\nDrawValues 0\r\nAutoUpdating 1\r\nDiscrete 0\r\nUseGlobalOpacity 1\r\nManualValuesSet 0\r\nClampedHigh 1\r\nClampedLow 1\r\nAlpha 1.000000e+000\r\nManualMin 0.000000e+000\r\nManualMax 1.000000e+000\r\nTextColor 1.000000 1.000000 1.000000\r\nColors 6\r\n0.300000 0.000000 0.500000\r\n0.000000 0.000000 1.000000\r\n0.000000 1.000000 0.000000\r\n1.000000 1.000000 0.000000\r\n1.000000 0.500000 0.000000\r\n1.000000 0.000000 0.000000\r\nPartitionValues\r\n0\r\nend_<Throughput>\r\nend_<Scales>\r\nend_<project>";
        public const string terrainGlobalFixedStr1 = "begin_<globals>\r\noffset_mode manual\r\nlongitude ";
        public const string terrainGlobalFixedStr2 =  "latitude ";
        public const string terrainGlobalFixedStr3 ="end_<globals>";
        public const string terrainFeatureFixedStr1 = "begin_<feature>";
        public const string terrainFeatureFixedStr2 = "feature ";
        public const string terrainFeatureFixedStr3 = "terrain\r\nactive";
        public const string terrainFeatureFixedStr4 = "filename ";
        public const string terrainFeatureFixedStr5 = "end_<feature>";
        //public const string terrainStudyAreaStr1 = "FirstAvailableStudyAreaNumber 0";
        public const string terrainStudyAreaStr1 = "begin_<studyarea> studyarea\r\nStudyAreaNumber 0";
        public const string terrainStudyAreaStr2 = "active\r\nautoboundary 1\r\nbegin_<model>\r\nfull3d\r\n"
                                                                       + "raytracingmode sbr\r\nend_<model>\r\nbegin_<boundary>\r\nbegin_<reference>\r\n"
                                                                       + "cartesian\r\nlongitude ";
        public const string terrainStudyAreaStr3 = "visible no\r\nterrain\r\nend_<reference>\r\nzmin ";
        public const string terrainStudyAreaStr4 = "zmax ";
        public const string terrainStudyAreaStr5 = "nVertices 4";
        public const string terrainStudyAreaStr6 = "end_<boundary>\r\nend_<studyarea>";

        public const string waveFormStr1 = "begin_<Waveform>";
        public const string waveFormStr2  = "end_<Waveform>";
        public const string waveFormStr3 = "FirstAvailableCommSystemNumber 0";

        public const string projectIndeStr = "begin_<project>";
        public const string waveIndeStr = "begin_<Waveform>";
        public const string antennaIndeStr = "begin_<antenna>";
        public const string terrainIndeStr = "<terrain>";
        public const string transmitterIndeStr = "begin_<transmitter>";
        public const string gridReceiverIndeStr = "begin_<grid>";
        public const string pointReceiverIndeStr = "begin_<points>";
        public const string longitudeIndeStr = "<longitude>";
        public const string latitudeIndeStr = "<latitude>";


        public const string resultSetIndeStr1 = "<Paths>";
        public const string resultSetIndeStr2 = "<Pathloss>";
        public const string resultSetIndeStr3 = "<EField>";
        public const string resultSetIndeStr4 = "<EFieldTotal>";
        public const string resultSetIndeStr5 = "<Power>";

        public const string resultSetOfTreeIndeStr1 = "传播路径";
        public const string resultSetOfTreeIndeStr2 = "路径损耗";
        public const string resultSetOfTreeIndeStr3 = "电场强度及相位";
        public const string resultSetOfTreeIndeStr4 = "总电场强度及相位";
        public const string resultSetOfTreeIndeStr5 = "接收功率";


        public const string antennaStr1 = "begin_<antenna>";
        public const string antennaStr2 = "component TotalGain\r\ngain_range 40\r\nshow_arrow no\r\nis_sphere no";
        public const string antennaStr3 = "end_<antenna>";



        //public const string vwFileStr = "begin_<States>\r\n"
        //                                            +"2d 1"+"\r\n"+"ortho 1"+"\r\n"+"wireframe 1"+"\r\n"
        //                                            +"normals 0"+"\r\n"+"globalOrigin 0\r\n"+"hilites 0\r\n"
        //                                            +"cities 1\r\n"+"terrain 1\r\n"+"foliage 1\r\n"+"floorplan 1\r\n"
        //                                            +"object 1\r\n"+"receivers 1\r\n"+"transmitters 1\r\n"+"images 1\r\n"
        //                                            +"studyarea 1\r\n"+"output 1\r\n"+"zscale off\r\n"+"zscale_factor 1\r\n"
        //                                            +"gridElevation 0\r\n"+"autoGridElevation on\r\n"+"gridspacing 0.000833333\r\n"
        //                                            +"gridQuadrants 0\r\n"+"grid 0\r\n"+"legend 0\r\n"+"descriptions on\r\n"
        //                                            +"description_backgrounds on\r\n"+"end_<States>\r\n"+"begin_<Rotations>\r\n"
        //                                            +"3dOrthographic 0 0 0\r\n"+"3dPerspective 0 0 0\r\n"+"2d 0 0 0\r\n"
        //                                            +"end_<Rotations>\r\n"+"begin_<Translations>\r\n"+"3dOrthographic 0 0 0\r\n"
        //                                            +"3dPerspective 0 0 0\r\n"+"2d 0 0 0\r\n"+"end_<Translations>\r\n"
        //                                            +"begin_<Scale>\r\n"+"3dOrthographic 1 1 1\r\n"+"3dPerspective 1 1 1\r\n"
        //                                            +"2d 1 1 1\r\n"+"end_<Scale>\r\n";
        public const string transmitterStr1Ofsetup = "begin_<transmitter>";
        public const string transmitterStr2Ofsetup = "filename ";
        public const string transmitterStr3Ofsetup = "FirstAvailableTxNumber ";
        public const string transmitterStr4Ofsetup = "end_<transmitter>";

        public const string transmitterStr1OfTr = "begin_<points>";
        public const string transmitterStr2OfTr = "TxSet ";
        public const string transmitterStr3OfTr = "active";
        public const string transmitterStr4OfTr = "vertical_line yes";
        public const string transmitterStr5OfTr = "cube_size ";
        public const string transmitterStr6OfTr = "CVxLength 10.00000";
        public const string transmitterStr7OfTr = "CVyLength 10.00000";
        public const string transmitterStr8OfTr = "CVzLength 10.00000";
        public const string transmitterStr9OfTr = "AutoPatternScale\r\nShowDescription yes\r\nCVsVisible no\r\nCVsThickness 3";
        public const string transmitterStr10OfTr = "begin_<location>";
        public const string transmitterStr11OfTr = "begin_<reference>";
        public const string transmitterStr12OfTr = "longitude ";
        public const string transmitterStr13OfTr = "latitude ";
        public const string transmitterStr14OfTr = "visible no";
        public const string transmitterStr15OfTr = "end_<reference>\r\nnVertices 1\r\n0.0000 0.0000 2.0000";
        public const string transmitterStr16OfTr = "end_<location>";
        //public const string transmitterStr17OfTr = "begin_<antenna>";
        public const string transmitterStr18OfTr = "rotation_x ";
        public const string transmitterStr19OfTr = "rotation_y ";
        public const string transmitterStr20OfTr = "rotation_z ";
        public const string transmitterStr21OfTr = "power ";
        public const string transmitterStr22OfTr = "end_<antenna>";
        public const string transmitterStr23OfTr = "pattern_show_arrow no\r\npattern_show_as_sphere no\r\ngenerate_p2p yes";
        public const string transmitterStr24OfTr = "end_<points>";

        public const string receiverOfSetupStr0 = "begin_<receiver>";
        public const string receiverOfSetupStr1 = "filename ";
        public const string receiverOfSetupStr2 = "FirstAvailableRxNumber ";
        public const string receiverOfSetupStr3 = "end_<receiver>";

        public const string gridReceiverOfRxStr0 = "begin_<grid>";
        public const string gridReceiverOfRxStr1 = "RxSet ";
        public const string gridReceiverOfRxStr2 = "active\r\nvertical_line yes\r\ncube_size 25.00000\r\nCVxLength 10.00000\r\nCVyLength 10.00000\r\n"
                                                                        + "CVzLength 10.00000\r\nAutoPatternScale\r\nShowDescription yes\r\n"
                                                                        + "CVsVisible no\r\nCVsThickness 3\r\nbegin_<location>\r\nbegin_<reference>\r\n"
                                                                        + "cartesian";

        public const string gridReceiverOfRxStr3 = "longitude ";
        public const string gridReceiverOfRxStr4 = "latitude ";
        public const string gridReceiverOfRxStr5 = "visible no";
        public const string gridReceiverOfRxStr6 = "end_<reference>";
        public const string gridReceiverOfRxStr7 = "side1 300.000\r\nside2 300.000";
        public const string gridReceiverOfRxStr8 = "spacing ";
        public const string gridReceiverOfRxStr9 = "nVertices 1\r\n0.000 0.000 2.000\r\nend_<location>";
        

        public const string gridReceiverOfRxStr10="power 0.00000\r\nend_<antenna>\r\n"
                                                                        + "begin_<sbr>\r\nbounding_box\r\nend_<sbr>\r\nNoiseFigure 3.00000\r\n"
                                                                        + "pattern_show_arrow no\r\npattern_show_as_sphere no\r\ngenerate_p2p no\r\n"
                                                                        + "end_<grid>";

        public const string gridReceiverOfRxStr11 = "end_<grid>";
        
        public const string gridpointReceiverOfRxStr0 = "rotation ";
        public const string gridpointReceiverOfRxStr1 = "rotation_x ";
        public const string gridpointReceiverOfRxStr2 = "rotation_y ";
        public const string gridpointReceiverOfRxStr3 = "rotation_z ";



        public const string pointReceiverOfRxStr0 = "begin_<points>";
        public const string pointReceiverOfRxStr1 = "RxSet ";
        public const string pointReceiverOfRxStr2 = "active";
        public const string pointReceiverOfRxStr3 = "vertical_line yes\r\ncube_size 25.00000\r\nCVxLength 10.00000\r\nCVyLength 10.00000\r\n"
                                                                        + "CVzLength 10.00000\r\nAutoPatternScale\r\nShowDescription yes\r\n"
                                                                        + "CVsVisible no\r\nCVsThickness 3\r\nbegin_<location>\r\nbegin_<reference>\r\n"
                                                                        + "cartesian";
        public const string pointReceiverOfRxStr4 = "visible no";
        public const string pointReceiverOfRxStr5 = "end_<reference>\r\nnVertices 1\r\n0.0000  0.0000  2.000\r\nend_<location>";
        public const string pointReceiverOfRxStr6 = "power 0.00000\r\nend_<antenna>\r\nNoiseFigure 3.00000\r\npattern_show_arrow no\r\n"
                                                                        + "pattern_show_as_sphere no\r\ngenerate_p2p yes\r\nend_<points>";



    }
}
