using HalconDotNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace test_serialize
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        HImage img = null;
        HRegion region = null;
        string b64StreamData = null;

        private void button1_Click(object sender, EventArgs e)
        {
            //read
            if (img == null) 
            {
                img = new HImage();
            }
            img.Dispose();
            img.ReadImage("printer_chip/printer_chip_01");
            img.GetImageSize(out HTuple width, out HTuple height);
            HOperatorSet.SetSystem("width",width);
            HOperatorSet.SetSystem ("height",height);
            hSmartWindowControl1.HalconWindow.ClearWindow();
            hSmartWindowControl1.HalconWindow.DispObj(img);
            hSmartWindowControl1.SetFullImagePart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(img == null)
            {
                return;
            }
            if(region == null)
            {
                region = new HRegion();
            }
            region.Dispose();
            region = img.Threshold(128.0, 255.0);

            hSmartWindowControl1.HalconWindow.ClearWindow();
            hSmartWindowControl1.HalconWindow.DispObj(img);
            hSmartWindowControl1.HalconWindow.SetColor("red");
            hSmartWindowControl1.HalconWindow.DispObj(region);
            hSmartWindowControl1.SetFullImagePart();
            b64StreamData = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            if (region == null)
            {
                return;
            }
            MemoryStream ms = new MemoryStream();
            HObject obj = new HObject(region);
            obj.Serialize(ms);
            ms.Flush();
            ms.Position = 0;
            byte[] bytearr = ms.ToArray();
            b64StreamData = Convert.ToBase64String(bytearr);
            ms.Close();
            obj.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            if (b64StreamData == null)
            {
                return;
            }
            hSmartWindowControl1.HalconWindow.ClearWindow();
            hSmartWindowControl1.HalconWindow.DispObj(img);
            hSmartWindowControl1.HalconWindow.SetColor("yellow");
            byte[] byteArray = Convert.FromBase64String(b64StreamData);
            MemoryStream memoryStream = new MemoryStream(byteArray);
            HObject reg = HObject.Deserialize(memoryStream);
            memoryStream.Close();
            hSmartWindowControl1.HalconWindow.DispObj(reg);
            hSmartWindowControl1.SetFullImagePart();
            reg.Dispose();
        }
    }
}
