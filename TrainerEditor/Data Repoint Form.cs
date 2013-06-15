using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TrainerEditor
{
    public partial class Data_Repoint_Form : Form
    {
        private uint _dataLength;
        private byte[] _rom;
        private TrainerEditor frm;
        private bool originalChecked;
        private uint _trainerLocation;
        
        public Data_Repoint_Form(uint dataLength, bool movesetStatus, byte[] rom, uint currentOffset, TrainerEditor form, uint trainerLocation)
        {
            InitializeComponent();
            _dataLength = dataLength;
            chkBxMovesets.Checked = movesetStatus;
            originalChecked = movesetStatus;
            lblOldDataLength.Text = "Old Data Length: 0x" + _dataLength.ToString("X");
            _rom = rom;
            upDownNumberOfPokemon_ValueChanged(new object(), new EventArgs());
            txtCurrentOffset.Text = "0x" + currentOffset.ToString("X");
            txtNewOffset.Text = "0x" + currentOffset.ToString("X");
            _trainerLocation = trainerLocation;
            frm = form;
            this.MinimumSize = this.MaximumSize = this.Size;
        }

        private void upDownNumberOfPokemon_ValueChanged(object sender, EventArgs e)
        {
            uint movesetIndex = 8;
            if (chkBxMovesets.Checked)
            {
                movesetIndex = 16;
            }
            uint newDataLength = (uint)(upDownNumberOfPokemon.Value * movesetIndex);
            if (newDataLength > _dataLength)
            {
                btnRepoint.Text = "Repoint";
                lblRepointNeeded.Text = "Repoint Required!";
                lblRepointNeeded.ForeColor = Color.Red;
                txtNewOffset.Enabled = true;
                btnFindFreeSpace.Enabled = true;
                txtFreeSpaceByte.Enabled = true;
            }
            else
            {
                btnRepoint.Text = "Change";
                lblRepointNeeded.Text = "Repoint Not Needed!";
                lblRepointNeeded.ForeColor = Color.Black;
                txtNewOffset.Enabled = false;
                btnFindFreeSpace.Enabled = false;
                txtFreeSpaceByte.Enabled = false;
            }
            lblNewDataLength.Text = "New Data Length: 0x" + newDataLength.ToString("X");
        }

        private void chkBxMovesets_CheckedChanged(object sender, EventArgs e)
        {
            upDownNumberOfPokemon_ValueChanged(sender, e);
        }

        private void btnFindFreeSpace_Click(object sender, EventArgs e)
        {
            uint searchStartLocation = 0;
            if (!txtNewOffset.Text.Equals(""))
            {
                bool success = UInt32.TryParse(frm.ToDecimal(txtNewOffset.Text), out searchStartLocation);
                if (!success)
                {
                    searchStartLocation = UInt32.Parse(frm.ToDecimal(txtCurrentOffset.Text));
                }
            }
            else
            {
                searchStartLocation = UInt32.Parse(frm.ToDecimal(txtCurrentOffset.Text));
            }
            uint result = 1;
            byte freeSpaceByte = 0xFF;
            bool success2 = Byte.TryParse(txtFreeSpaceByte.Text, out freeSpaceByte);
            if (!success2)
            {
                success2 = Byte.TryParse(frm.ToDecimal(txtFreeSpaceByte.Text), out freeSpaceByte);
                if (!success2)
                {
                    freeSpaceByte = 0xFF;
                }
            }
            while (result % 4 != 0)
            {
                while (searchStartLocation % 4 != 0)
                {
                    searchStartLocation++;
                }
                result = FindFreeSpace(UInt32.Parse(frm.ToDecimal(lblNewDataLength.Text.Split(' ')[3])), searchStartLocation, _rom, freeSpaceByte);
                searchStartLocation++;
            }
            txtNewOffset.Text = "0x" + result.ToString("X");
        }

        private uint FindFreeSpace(uint dataSize, uint startLocation, byte[] rom, byte freeSpaceByte)
        {
            bool spaceFound = false;
            while (!spaceFound)
            {
                for (int i = 0; i <= dataSize; i++)
                {
                    if (startLocation + dataSize <= rom.Length)
                    {
                        if (rom[startLocation + i] != freeSpaceByte)
                        {
                            break;
                        }
                        else if (i == dataSize)
                        {
                            spaceFound = true;
                            break;
                        }
                    }
                    else
                    {
                        spaceFound = true;
                        MessageBox.Show("No free space could be found in the alloted area. Try widening the search parameters.");
                        startLocation = Int32.MaxValue;
                        break;
                    }
                }
                if (!spaceFound)
                {
                    startLocation++;
                }
            }
            return startLocation;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRepoint_Click(object sender, EventArgs e)
        {
            uint numberOfPokemon = _dataLength;
            if (originalChecked)
            {
                numberOfPokemon >>= 4;
            }
            else
            {
                numberOfPokemon >>= 3;
            }
            uint newBaseLocation = 0;
            uint originalLocation = UInt32.Parse(frm.ToDecimal(txtCurrentOffset.Text));
            bool success = false;
            if (!txtNewOffset.Text.Equals(""))
            {
                success = UInt32.TryParse(txtNewOffset.Text, out newBaseLocation);
                if (!success)
                {
                    success = UInt32.TryParse(frm.ToDecimal(txtNewOffset.Text), out newBaseLocation);
                    if (!success)
                    {
                        MessageBox.Show("Failed to parse location of new data!");
                    }
                }
            }
            else
            {
                newBaseLocation = FindFreeSpace(UInt32.Parse(frm.ToDecimal(lblNewDataLength.Text.Split(' ')[3])), originalLocation, _rom, 0xFF);
                chkBxRemoveOriginalData.Checked = true;
                success = true;
            }
            uint backupNewBaseLocation = newBaseLocation;
            if (success)
            {
                for (uint i = 0; i < numberOfPokemon; i++)
                {
                    frm.WriteToROM(newBaseLocation, 2, (uint)(_rom[originalLocation] | (_rom[originalLocation + 1] << 8)));
                    if (chkBxRemoveOriginalData.Checked)
                    {
                        frm.WriteToROM(originalLocation, 2, (uint)0xFFFF);
                    }
                    originalLocation += 2;
                    newBaseLocation += 2;
                    frm.WriteToROM(newBaseLocation, 2, (uint)(_rom[originalLocation] | (_rom[originalLocation + 1] << 8)));
                    if (chkBxRemoveOriginalData.Checked)
                    {
                        frm.WriteToROM(originalLocation, 2, (uint)0xFFFF);
                    }
                    originalLocation += 2;
                    newBaseLocation += 2;
                    frm.WriteToROM(newBaseLocation, 2, (uint)(_rom[originalLocation] | (_rom[originalLocation + 1] << 8)));
                    if (chkBxRemoveOriginalData.Checked)
                    {
                        frm.WriteToROM(originalLocation, 2, (uint)0xFFFF);
                    }
                    originalLocation += 2;
                    newBaseLocation += 2;
                    frm.WriteToROM(newBaseLocation, 2, (uint)(_rom[originalLocation] | (_rom[originalLocation + 1] << 8)));
                    if (chkBxRemoveOriginalData.Checked)
                    {
                        frm.WriteToROM(originalLocation, 2, (uint)0xFFFF);
                    }
                    originalLocation += 2;
                    newBaseLocation += 2;
                    if (chkBxMovesets.Checked)
                    {
                        if (originalChecked)
                        {
                            for (uint j = 0; j < 4; j++)
                            {
                                frm.WriteToROM(newBaseLocation, 2, (uint)(_rom[originalLocation] | (_rom[originalLocation + 1] << 8)));
                                if (chkBxRemoveOriginalData.Checked)
                                {
                                    frm.WriteToROM(originalLocation, 2, (uint)0xFFFF);
                                }
                                originalLocation += 2;
                                newBaseLocation += 2;
                            }
                        }
                        else
                        {
                            for (uint j = 0; j < 4; j++)
                            {
                                frm.WriteToROM(newBaseLocation, 2, 0x99);
                                originalLocation += 2;
                                newBaseLocation += 2;
                            }
                        }
                    }
                    else
                    {
                        if (originalChecked)
                        {
                            originalLocation += 8;
                        }
                    }
                }
                for (uint i = 0; i < upDownNumberOfPokemon.Value - numberOfPokemon; i++)
                {
                    for (uint j = 0; j < 4; j++)
                    {
                        frm.WriteToROM(newBaseLocation, 2, 1);
                        newBaseLocation += 2;
                    }
                    if (chkBxMovesets.Checked)
                    {
                        for (uint j = 0; j < 4; j++)
                        {
                            frm.WriteToROM(newBaseLocation, 2, 0x99);
                            newBaseLocation += 2;
                        }
                    }
                }
                frm.WriteToROM(_trainerLocation + 0x24, 4, backupNewBaseLocation + 0x08000000);
                frm.WriteToROM(_trainerLocation + 0x20, 4, (uint)upDownNumberOfPokemon.Value);
                if (chkBxMovesets.Checked)
                {
                    frm.WriteToROM(_trainerLocation, 1, (uint)(_rom[_trainerLocation] | 1));
                }
                frm.UpdateTrainerData();
                this.Close();
            }
        }
    }
}
