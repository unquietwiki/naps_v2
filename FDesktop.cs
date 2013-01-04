// Not Another PDF Scanner v2
// Original Copyright © 2009 Pavel Sorejs, http://sourceforge.net/projects/naps/
// Updated by Michael Adams, http://unquietwiki.com, 2012
// Updated version uses LPGL-licensed "PDF Clown": http://www.stefanochizzolini.it/en/projects/clown/index.html

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Drawing.Imaging;
using NAPS.wia;
using NAPS.twain;
using WIA;
using org.pdfclown.documents;
using org.pdfclown.documents.contents;
using org.pdfclown.documents.contents.xObjects;
using org.pdfclown.documents.contents.colorSpaces;
using org.pdfclown.documents.contents.composition;
using org.pdfclown.tools;
using org.pdfclown.files;

namespace NAPS
{
    public partial class FDesktop : Form
    {
        private SortedList<int,CScannedImage> images;
        public FDesktop()
        {
            InitializeComponent();
            images = new SortedList<int, CScannedImage>();
        }

        private void thumbnailList1_ItemActivate(object sender, EventArgs e)
        {
            FViewer viewer = new FViewer(images[(int)thumbnailList1.SelectedItems[0].Tag].BaseImage);
            viewer.ShowDialog();
        }

        private void updateView()
        {
            thumbnailList1.UpdateView(images);
        }

        private void scanWIA(CScanSettings Profile)
        {
            CWIAAPI api;
            try
            {
                api = new CWIAAPI(Profile);
            }
            catch (Exceptions.EScannerNotFound)
            {
                MessageBox.Show("Device not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CScannedImage img = api.GetImage();

            if (img != null)
            {
                int next = images.Count > 0 ? images.Keys[images.Count - 1] + 1 : 0;
                images.Add(next, img);
            }
            thumbnailList1.UpdateImages(images);
            Application.DoEvents();

            if (Profile.Source != CScanSettings.ScanSource.GLASS)
            {
                while (img != null)
                {
                    img = api.GetImage();
                    if (img != null)
                    {
                        int next = images.Count > 0 ? images.Keys[images.Count - 1] + 1 : 0;
                        images.Add(next, img);
                    }
                    thumbnailList1.UpdateImages(images);
                    Application.DoEvents();
                }
            }
        }

        private void scanTWAIN(string DeviceName)
        {
            CTWAINAPI twa;
            try
            {
                twa = new CTWAINAPI(DeviceName,this);
            }
            catch (Exceptions.EScannerNotFound)
            {
                MessageBox.Show("Device not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<CScannedImage> scanned = twa.Scan();
            foreach (CScannedImage bmp in scanned)
            {
                int next = images.Count > 0 ? images.Keys[images.Count - 1] + 1 : 0;
                images.Add(next, bmp);
            }
            thumbnailList1.UpdateImages(images);
        }

        private void deleteItems()
        {
            if (thumbnailList1.SelectedItems.Count > 0)
            {
                if (MessageBox.Show(string.Format("Do you really want to delete {0} items?", thumbnailList1.SelectedItems.Count), "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    foreach (ListViewItem delitem in thumbnailList1.SelectedItems)
                    {
                        images.Remove((int)delitem.Tag);
                    }
                    thumbnailList1.UpdateImages(images);
                }
            }
        }

        private int getImageBefore(int id)
        {
            int ret = 0;
            foreach (int cid in images.Keys)
            {
                if (cid == id)
                    return ret;
                ret = cid;
            }
            return 0;
        }

        private int getImageAfter(int id)
        {
            bool found = false;
            foreach (int cid in images.Keys)
            {
                if (found)
                    return cid;
                if (cid == id)
                    found = true;
            }
            return 0;
        }

        private void moveUp()
        {
            if (thumbnailList1.SelectedItems.Count > 0)
            {
                if (thumbnailList1.SelectedItems[0].Index > 0)
                {
                    foreach (ListViewItem it in thumbnailList1.SelectedItems)
                    {
                        int before = getImageBefore((int)it.Tag);
                        CScannedImage temp = images[before];
                        images[before] = images[(int)it.Tag];
                        images[(int)it.Tag] = temp;
                        thumbnailList1.Items[thumbnailList1.Items.IndexOf(it) - 1].Selected = true;
                        it.Selected = false;
                    }
                    updateView();
                }
            }
        }

        private void moveDown()
        {
            if (thumbnailList1.SelectedItems.Count > 0)
            {
                if (thumbnailList1.SelectedItems[thumbnailList1.SelectedItems.Count - 1].Index < images.Count - 1)
                {

                    for (int i = thumbnailList1.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        ListViewItem it = thumbnailList1.SelectedItems[i];
                        int after = getImageAfter((int)it.Tag);
                        CScannedImage temp = images[after];
                        images[after] = images[(int)it.Tag];
                        images[(int)it.Tag] = temp;
                        thumbnailList1.Items[thumbnailList1.Items.IndexOf(it) + 1].Selected = true;
                        it.Selected = false;
                    }
                    updateView();
                }
            }
        }

        private void rotateLeft()
        {
        	rotateItem(RotateFlipType.Rotate270FlipNone);
        }

        private void rotateRight()
        {
        	rotateItem(RotateFlipType.Rotate90FlipNone);
        }
        
        private void rotateFlip()
        {
        	rotateItem(RotateFlipType.RotateNoneFlipXY);
        }
        
        //Master function for image/document rotation
        //
        //Check to see if there's images, then allow flip. If images, but none pick, flip all of them.
        //
        private void rotateItem(RotateFlipType ft)
        {
        	if (thumbnailList1.Items.Count > 0)
        	{
            	if (thumbnailList1.SelectedItems.Count > 0)
            	{
                	foreach (ListViewItem it in thumbnailList1.SelectedItems)
                	{ 
                	    images[(int)it.Tag].RotateFlip(ft);
                	}
                	updateView();
            	}
            	else
            	{
            		foreach (ListViewItem it in thumbnailList1.Items)
            		{
            			images[(int)it.Tag].RotateFlip(ft);
            		}
            		updateView();
            	}
        	}        	
        }

        private void thumbnailList1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    deleteItems();
                    break;
                case Keys.Left:
                    if (e.Control)
                        moveUp();
                    break;
                case Keys.Right:
                    if (e.Control)
                        moveDown();
                    break;
            }
        }

        private void exportPDFProcess(string filename, FPDFSave dialog)
        {
        	org.pdfclown.files.File file = new org.pdfclown.files.File();
      		org.pdfclown.documents.Document document = file.Document;           	
      		Page page;
      		Stream stream;      		
      		org.pdfclown.documents.contents.entities.JpegImage currentImage;
      		int i = 1;
      		
      		foreach (CScannedImage img in images.Values)
      		{
                ThreadStart setstatus = delegate { dialog.SetStatus(i, images.Count); };                
                dialog.Invoke(setstatus);
                Size pageSize = new Size((int)(img.BaseImage.Width / img.BaseImage.HorizontalResolution * 72),(int)(img.BaseImage.Height / img.BaseImage.VerticalResolution * 72));
                PointF point = new PointF(0,0);
                Resources resources = new Resources(document);                
                //page = new Page(document,pageSize,resources);
                page = new Page(document);
                document.Pages.Add(page);
                stream = new MemoryStream();                
                img.BaseImage.Save(stream,ImageFormat.Jpeg);
                PrimitiveComposer composer = new PrimitiveComposer(page);                
                currentImage = new org.pdfclown.documents.contents.entities.JpegImage(stream);
                composer.ShowXObject(currentImage.ToXObject(document),point,pageSize);                
                stream.Flush();
                composer.Flush();                
				i++;
      		}
      		file.Save(filename,SerializationModeEnum.Standard);
      		dialog.Invoke(new ThreadStart(dialog.Close));
        }

        private void exportPDF(string filename)
        {
            FPDFSave pdfdialog = new FPDFSave();
            ThreadStart starter = delegate { exportPDFProcess(filename, pdfdialog); };
            new Thread(starter).Start();
            pdfdialog.ShowDialog(this);
        }

        private void tsScan_Click(object sender, EventArgs e)
        {
            FChooseProfile prof = new FChooseProfile();
            prof.ShowDialog();

            if (prof.Profile == null)
                return;

            if (prof.Profile.DeviceDriver == CScanSettings.Driver.WIA)
                scanWIA(prof.Profile);
            else
                scanTWAIN(prof.Profile.DeviceID);
        }

        private void tsSavePDF_Click(object sender, EventArgs e)
        {
            if (images.Count > 0)
            {

                SaveFileDialog sd = new SaveFileDialog();
                sd.OverwritePrompt = true;
                sd.AddExtension = true;
                sd.Filter = "PDF document (*.pdf)|*.pdf";                
                sd.FileName = randomFilename();

                if (sd.ShowDialog() == DialogResult.OK)
                {
                    exportPDF(sd.FileName);
                }
            }
        }

        private void tsSaveImage_Click(object sender, EventArgs e)
        {
            if (images.Count > 0)
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.OverwritePrompt = true;
                sd.AddExtension = true;
                sd.Filter = "Bitmap Files (*.bmp)|*.bmp" +
                "|Enhanced Windows MetaFile (*.emf)|*.emf" +
                "|Exchangeable Image File (*.exif)|*.exif" +
                "|Gif Files (*.gif)|*.gif" +
				"|JPEG Files (*.jpg)|*.jpg" +
                "|PNG Files (*.png)|*.png" +
                "|TIFF Files (*.tif)|*.tif";
                sd.DefaultExt = "jpg";
                sd.FilterIndex = 5;
                sd.FileName = randomFilename();

                if (sd.ShowDialog() == DialogResult.OK)
                {

                    //Save images based on the selected format
                    switch(sd.FilterIndex)
                    {
                    	case 7:                    		
                        	Image[] imgs = new Image[images.Count];
                        	int i = 0;
                        	foreach (CScannedImage img in images.Values)
                        	{
                            	imgs[i] = img.BaseImage;
                            	i++;
                        	}
                        	CTiffHelper.SaveMultipage(imgs, sd.FileName);
                        	return;                        	
                    	case 1:
                        	saveImages(sd.FileName,ImageFormat.Bmp);
                        	break;
                    	case 2:
                        	saveImages(sd.FileName,ImageFormat.Emf);
                        	break;
                    	case 3:
                        	saveImages(sd.FileName,ImageFormat.Exif);
                        	break;
                    	case 4:
                        	saveImages(sd.FileName,ImageFormat.Gif);
                        	break;
                    	case 6:
                        	saveImages(sd.FileName,ImageFormat.Png);
                        	break;
                    	case 5:
                    	default:
                        	saveImages(sd.FileName,ImageFormat.Jpeg);
                        	break;                        	
                    }
                }
            }
        }
        
        private void tsPDFEmail_Click(object sender, EventArgs e)
        {
            if (images.Count > 0)
            {
                string path = Application.StartupPath + "\\Scan.pdf";
                exportPDF(path);
                MAPI.CMAPI.SendMail(path, "");
                System.IO.File.Delete(path);
            }
        }

        private void tsMoveUp_Click(object sender, EventArgs e)
        {
            moveUp();
        }

        private void tsMoveDown_Click(object sender, EventArgs e)
        {
            moveDown();
        }

        private void tsRotateLeft_Click(object sender, EventArgs e)
        {
            rotateLeft();
        }

        private void tsRotateRight_Click(object sender, EventArgs e)
        {
            rotateRight();
        }

        private void tsProfiles_Click(object sender, EventArgs e)
        {
            FManageProfiles pmanager = new FManageProfiles();
            pmanager.ShowDialog();
        }

        private void tsAbout_Click(object sender, EventArgs e)
        {
            new FAbout().ShowDialog();
        }

        private void tsExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        //Generate a random filename
        private string randomFilename()
        {
        	return (Environment.MachineName + "_" + Environment.UserName + "_" + System.DateTime.Now.ToFileTime());
        }
        
        //Image saving function to use with tsSaveImage
        private void saveImages(string filename, ImageFormat imgfmt)
        {
        	if (images.Count == 1)
            {
            	images.Values[0].BaseImage.Save(filename);
                return;
			}

			int i = 0;
			foreach (CScannedImage img in images.Values)
			{
				string filename2 = Path.GetDirectoryName(filename) + "\\" + Path.GetFileNameWithoutExtension(filename) + "_" + i.ToString().PadLeft(3, '0') + Path.GetExtension(filename);
				img.BaseImage.Save(filename2,imgfmt);				
				i++;
			}
        }
        
        
        void TsRotateFlipClick(object sender, EventArgs e)
        {
        	rotateFlip();
        }
    }
}