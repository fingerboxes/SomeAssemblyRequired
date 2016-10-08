/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2016 Alexander Taylor
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
// xcopy /Y $(TargetPath) $(TargetDir)..\..\..\GameData\SomeAssemblyRequired\Plugins\

using System;
using System.IO;

using UnityEngine;
using KSPPluginFramework;

namespace SomeAssemblyRequired
{
    [KSPAddon(KSPAddon.Startup.EditorAny, true)]
    public class EditorModule : MonoBehaviour
    {

        public static string shipFilename = "Test";
        void Start()
        {
            EditorLogic.fetch.launchBtn.onClick.RemoveAllListeners();
            EditorLogic.fetch.launchBtn.onClick.AddListener(delegate { OnLaunchClick(); });
        }

        public void OnLaunchClick()
        {
            PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f),
                    new MultiOptionDialog("Do you want to build this craft?",
                        "Build Craft",
                        HighLogic.UISkin,
                        new Rect(0.5f, 0.5f, 350f, 200f),
                        new DialogGUIFlexibleSpace(),
                        new DialogGUIHorizontalLayout(
                            new DialogGUIFlexibleSpace(),
                            new DialogGUIVerticalLayout(
                                new DialogGUIFlexibleSpace(),
                                new DialogGUILabel("Estimated Build Cost"),
                                new DialogGUIFlexibleSpace()
                            ),
                            new DialogGUIFlexibleSpace(),
                            new DialogGUIFlexibleSpace(),
                            new DialogGUIVerticalLayout(
                                new DialogGUIFlexibleSpace(),
                                new DialogGUIBox("999,999,999,9999", 200f, 30f),
                                new DialogGUIFlexibleSpace()
                            )                            
                        ),
                        new DialogGUIHorizontalLayout(
                            new DialogGUIFlexibleSpace(),
                            new DialogGUIVerticalLayout(
                                new DialogGUIFlexibleSpace(),
                                new DialogGUILabel("Estimated Build Time"),
                                new DialogGUIFlexibleSpace()
                            ),
                            new DialogGUIFlexibleSpace(),
                            new DialogGUIFlexibleSpace(),
                            new DialogGUIVerticalLayout(
                                new DialogGUIFlexibleSpace(),
                                new DialogGUIBox("999 Years, 999 Days, 99 Min, 99 Sec", 200f, 30f),
                                new DialogGUIFlexibleSpace()
                            )
                        ),
                        new DialogGUIFlexibleSpace(),
                        new DialogGUIHorizontalLayout(
                            new DialogGUIFlexibleSpace(),
                            new DialogGUIButton("Confirm",
                                delegate
                                {
                                    SaveShip();                                    
                                }, 140.0f, 30.0f, true),
                            new DialogGUIButton("Cancel", () => { }, 140.0f, 30.0f, true)
                            )),
                    false,
                    HighLogic.UISkin);
        }

        public class DialogGUILineItem : DialogGUIBase
        {
            public DialogGUILineItem(string message, string value)
            {
                
            }
        }

        public void SaveShip()
        {
            ShipConstruct ship = EditorLogic.fetch.ship;
            string directoryPath = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/SAR/Pending/" + ShipConstruction.GetShipsSubfolderFor(EditorDriver.editorFacility) + "/";
            string fullSavePath = directoryPath + ship.shipName + "_" + Math.Round(Planetarium.GetUniversalTime());

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            int i = 0;
            while (File.Exists(fullSavePath + "_" + i + ".craft"))
            {
                i++;
            }

            fullSavePath = fullSavePath + "_" + i + ".craft";

            ship.SaveShip().Save(fullSavePath);
        }
    }
}
