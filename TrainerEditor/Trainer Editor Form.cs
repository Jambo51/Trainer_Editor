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
    public partial class TrainerEditor : Form
    {
        private string chosenRom;
        private byte[] rom;
        private uint trainerTable;
        private ushort numberOfTrainers;
        private uint itemTable;
        private ushort numberOfItems;
        private uint pokemonNamesLocation;
        private ushort numberOfPokemon;
        private uint movesNamesLocation;
        private ushort numberOfMoves;
        private uint classNamesLocation;
        private byte numberOfClasses;
        private Dictionary<byte, char> characterValues;
        private Dictionary<int, string> nameLookUp;
        private bool doit = true;
        private bool doit2 = true;
        private bool doit3 = false;
        private bool doit4 = true;
        private bool doit5 = true;
        private bool doit6 = true;
        private bool doit7 = true;
        private bool loading = false;
        private bool firstOpen = false;
        private bool unsaved = false;
        private List<string> table;
        private List<string> pokemonTable;
        private List<string> classesTable;
        private uint dataLength = 0;
        private uint numberOfEVsActive = 0;
        private bool romOpen = false;
        private uint trainerImageTable;
        private uint trainerPaletteTable;
        private bool showBackgroundColours = false;
        private uint classMoneyLocation;
        private bool foundClassMoney;
        
        public TrainerEditor()
        {
            InitializeComponent();
            characterValues = ReadTableFile(System.Windows.Forms.Application.StartupPath + @"\Table.ini");
            this.KeyDown += new KeyEventHandler(Form_KeyDown);
            this.txtLevel.LostFocus += new EventHandler(txtLevel_LostFocus);
            this.txtIVs.LostFocus += new EventHandler(txtIVs_LostFocus);
            this.txtName.LostFocus += new EventHandler(txtName_LostFocus);
            this.MinimumSize = this.MaximumSize = this.Size;
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case (Keys.S):
                        {
                            saveToolStripMenuItem_Click(sender, e);
                            break;
                        }
                    case (Keys.O):
                        {
                            openToolStripMenuItem_Click(sender, e);
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            unsaved = true;
            firstOpen = true;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                chosenRom = openFileDialog1.FileName;
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    rom = System.IO.File.ReadAllBytes(chosenRom);
                    ParseINI(System.IO.File.ReadAllLines(System.Windows.Forms.Application.StartupPath + @"\TrainerEditor.ini"), GetROMCode(rom));
                    loading = true;
                    comboBoxPokemonSpecies.Items.Clear();
                    comboBoxKnownMoves1.Items.Clear();
                    comboBoxKnownMoves2.Items.Clear();
                    comboBoxKnownMoves3.Items.Clear();
                    comboBoxKnownMoves4.Items.Clear();
                    comboBoxHeldItem.Items.Clear();
                    toolStripProgressBar1.Maximum = numberOfClasses + numberOfItems + numberOfMoves + numberOfPokemon + numberOfTrainers;
                    toolStripStatusLabel1.Text = "Loading Trainer Data... (" + numberOfTrainers.ToString() + ")";
                    nameLookUp = new Dictionary<int, string>();
                    table = new List<string>();
                    for (uint i = 1; i <= numberOfTrainers; i++)
                    {
                        string name = ROMCharactersToString(10, 0x28 * (i - 1) + trainerTable + 4);
                        table.Add(i.ToString("X3") + " - " + name);
                        nameLookUp.Add((int)(i - 1), name);
                        toolStripProgressBar1.PerformStep();
                    }
                    lstBoxTrainers.DataSource = table;
                    toolStripStatusLabel1.Text = "Loading Item Names... (" + numberOfItems.ToString() + ")";
                    for (uint i = 1; i <= numberOfItems; i++)
                    {
                        string name = ROMCharactersToString(13, 0x2C * (i - 1) + itemTable);
                        comboBoxTrainerItem1.Items.Add(name);
                        comboBoxTrainerItem2.Items.Add(name);
                        comboBoxTrainerItem3.Items.Add(name);
                        comboBoxTrainerItem4.Items.Add(name);
                        comboBoxHeldItem.Items.Add(name);
                        toolStripProgressBar1.PerformStep();
                    }
                    toolStripStatusLabel1.Text = "Loading Pokémon Species Names... (" + numberOfPokemon.ToString() + ")";
                    for (uint i = 0; i <= numberOfPokemon; i++)
                    {
                        comboBoxPokemonSpecies.Items.Add(ROMCharactersToString(10, (uint)(0xB * i + pokemonNamesLocation)));
                        toolStripProgressBar1.PerformStep();
                    }
                    toolStripStatusLabel1.Text = "Loading Move Names... (" + numberOfMoves.ToString() + ")";
                    for (uint i = 1; i <= numberOfMoves; i++)
                    {
                        string name = ROMCharactersToString(13, 0xD * (i - 1) + movesNamesLocation);
                        comboBoxKnownMoves1.Items.Add(name);
                        comboBoxKnownMoves2.Items.Add(name);
                        comboBoxKnownMoves3.Items.Add(name);
                        comboBoxKnownMoves4.Items.Add(name);
                        toolStripProgressBar1.PerformStep();
                    }
                    toolStripStatusLabel1.Text = "Loading Class Names... (" + numberOfClasses.ToString() + ")";
                    classesTable = new List<string>();
                    for (byte b = 0; b < numberOfClasses; b++)
                    {
                        classesTable.Add(ROMCharactersToString(13, (uint)(0xD * b + classNamesLocation)) + " (0x" + b.ToString("X") + ")");
                        toolStripProgressBar1.PerformStep();
                    }
                    comboBoxClasses.DataSource = classesTable;
                    toolStripProgressBar1.Value = 0;
                    toolStripStatusLabel1.Text = "";
                    lstBoxTrainers.Enabled = true;
                    txtSearchNumber.Enabled = true;
                    txtSearchName.Enabled = true;
                    loading = false;
                    lstBoxTrainers_SelectedIndexChanged(new object(), new EventArgs());
                    romOpen = true;
                    this.Cursor = Cursors.Default;
                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Unable to open the ROM.");
                }
                catch
                {
                    MessageBox.Show("There was an error parsing the Pokémon data.");
                }
                firstOpen = false;
                this.Cursor = Cursors.Default;
            }
        }

        private string GetROMCode(byte[] file)
        {
            string toReturn = "";
            for (int i = 0; i < 4; i++)
            {
                toReturn += Convert.ToChar(file[0xAC + i]);
            }
            return toReturn;
        }

        private void ParseINI(string[] iniFile, string romCode)
        {
            bool getValues = false;
            foreach (string s in iniFile)
            {
                if (s.Equals("[" + romCode + "]"))
                {
                    getValues = true;
                    continue;
                }
                if (s.StartsWith("EVHack"))
                {
                    try
                    {
                        jambo51EVHackToolStripMenuItem.Checked = Boolean.Parse(s.Split('=')[1]);
                        jambo51EVHackToolStripMenuItem_Click(new object(), new EventArgs());
                    }
                    catch
                    {
                        jambo51EVHackToolStripMenuItem.Checked = false;
                        jambo51EVHackToolStripMenuItem_Click(new object(), new EventArgs());
                    }
                }
                if (getValues)
                {
                    if (s.Equals(@"[/" + romCode + "]"))
                    {
                        break;
                    }
                    else
                    {
                        if (s.StartsWith("MoveNames"))
                        {
                            bool success = UInt32.TryParse(s.Split('=')[1], out movesNamesLocation);
                            if (!success)
                            {
                                success = UInt32.TryParse(ToDecimal(s.Split('=')[1]), out movesNamesLocation);
                                if (!success)
                                {
                                    MessageBox.Show("Error parsing value for move names location!");
                                    break;
                                }
                            }
                        }
                        else if (s.StartsWith("NumberOfMoves"))
                        {
                            bool success = UInt16.TryParse(s.Split('=')[1], out numberOfMoves);
                            if (!success)
                            {
                                success = UInt16.TryParse(ToDecimal(s.Split('=')[1]), out numberOfMoves);
                                if (!success)
                                {
                                    MessageBox.Show("Error parsing value for number of moves!");
                                    break;
                                }
                            }
                        }
                        else if (s.StartsWith("ItemData"))
                        {
                            bool success = UInt32.TryParse(s.Split('=')[1], out itemTable);
                            if (!success)
                            {
                                success = UInt32.TryParse(ToDecimal(s.Split('=')[1]), out itemTable);
                                if (!success)
                                {
                                    MessageBox.Show("Error parsing value for move names location!");
                                    break;
                                }
                            }
                        }
                        else if (s.StartsWith("NumberOfItems"))
                        {
                            bool success = UInt16.TryParse(s.Split('=')[1], out numberOfItems);
                            if (!success)
                            {
                                success = UInt16.TryParse(ToDecimal(s.Split('=')[1]), out numberOfItems);
                                if (!success)
                                {
                                    MessageBox.Show("Error parsing value for number of moves!");
                                    break;
                                }
                            }
                        }
                        else if (s.StartsWith("PokemonNames"))
                        {
                            bool success = UInt32.TryParse(s.Split('=')[1], out pokemonNamesLocation);
                            if (!success)
                            {
                                success = UInt32.TryParse(ToDecimal(s.Split('=')[1]), out pokemonNamesLocation);
                                if (!success)
                                {
                                    MessageBox.Show("Error parsing value for move names location!");
                                    break;
                                }
                            }
                        }
                        else if (s.StartsWith("NumberOfPokemon"))
                        {
                            bool success = UInt16.TryParse(s.Split('=')[1], out numberOfPokemon);
                            if (!success)
                            {
                                success = UInt16.TryParse(ToDecimal(s.Split('=')[1]), out numberOfPokemon);
                                if (!success)
                                {
                                    MessageBox.Show("Error parsing value for number of moves!");
                                    break;
                                }
                            }
                        }
                        else if (s.StartsWith("TrainerTable"))
                        {
                            bool success = UInt32.TryParse(s.Split('=')[1], out trainerTable);
                            if (!success)
                            {
                                success = UInt32.TryParse(ToDecimal(s.Split('=')[1]), out trainerTable);
                                if (!success)
                                {
                                    MessageBox.Show("Error parsing value for move names location!");
                                    break;
                                }
                            }
                        }
                        else if (s.StartsWith("NumberOfTrainers"))
                        {
                            bool success = UInt16.TryParse(s.Split('=')[1], out numberOfTrainers);
                            if (!success)
                            {
                                success = UInt16.TryParse(ToDecimal(s.Split('=')[1]), out numberOfTrainers);
                                if (!success)
                                {
                                    MessageBox.Show("Error parsing value for number of moves!");
                                    break;
                                }
                            }
                        }
                        else if (s.StartsWith("ClassNamesLocation"))
                        {
                            bool success = UInt32.TryParse(s.Split('=')[1], out classNamesLocation);
                            if (!success)
                            {
                                success = UInt32.TryParse(ToDecimal(s.Split('=')[1]), out classNamesLocation);
                                if (!success)
                                {
                                    MessageBox.Show("Error parsing value for move names location!");
                                    break;
                                }
                            }
                        }
                        else if (s.StartsWith("NumberOfClassNames"))
                        {
                            bool success = Byte.TryParse(s.Split('=')[1], out numberOfClasses);
                            if (!success)
                            {
                                success = Byte.TryParse(ToDecimal(s.Split('=')[1]), out numberOfClasses);
                                if (!success)
                                {
                                    MessageBox.Show("Error parsing value for number of moves!");
                                    break;
                                }
                            }
                        }
                        else if (s.StartsWith("TrainerImageTable"))
                        {
                            bool success = UInt32.TryParse(s.Split('=')[1], out trainerImageTable);
                            if (!success)
                            {
                                success = UInt32.TryParse(ToDecimal(s.Split('=')[1]), out trainerImageTable);
                                if (!success)
                                {
                                    MessageBox.Show("Error parsing value for move names location!");
                                    break;
                                }
                            }
                        }
                        else if (s.StartsWith("NumberOfTrainerImages"))
                        {
                            ushort temp = 0;
                            bool success = UInt16.TryParse(s.Split('=')[1], out temp);
                            if (!success)
                            {
                                success = UInt16.TryParse(ToDecimal(s.Split('=')[1]), out temp);
                                if (!success)
                                {
                                    MessageBox.Show("Error parsing value for number of moves!");
                                    break;
                                }
                            }
                            upDownSprite.Maximum = (decimal)temp;
                        }
                        else if (s.StartsWith("TrainerPaletteTable"))
                        {
                            bool success = UInt32.TryParse(s.Split('=')[1], out trainerPaletteTable);
                            if (!success)
                            {
                                success = UInt32.TryParse(ToDecimal(s.Split('=')[1]), out trainerPaletteTable);
                                if (!success)
                                {
                                    MessageBox.Show("Error parsing value for move names location!");
                                    break;
                                }
                            }
                        }
                        else if (s.StartsWith("ClassMoneyLocation"))
                        {
                            bool success = UInt32.TryParse(s.Split('=')[1], out classMoneyLocation);
                            if (!success)
                            {
                                success = UInt32.TryParse(ToDecimal(s.Split('=')[1]), out classMoneyLocation);
                                if (!success)
                                {
                                    MessageBox.Show("Error parsing value for move names location!");
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public string ToDecimal(string input)
        {
            if (input.ToLower().StartsWith("0x") || input.ToUpper().StartsWith("&H"))
            {
                return Convert.ToUInt32(input.Substring(2), 16).ToString();
            }
            else if (input.ToLower().StartsWith("0o"))
            {
                return Convert.ToUInt32(input.Substring(2), 8).ToString();
            }
            else if (input.ToLower().StartsWith("0b"))
            {
                return Convert.ToUInt32(input.Substring(2), 2).ToString();
            }
            else if (input.ToLower().StartsWith("0t"))
            {
                return ThornalToDecimal(input.Substring(2));
            }
            else if ((input.StartsWith("[") && input.EndsWith("]")) || (input.StartsWith("{") && input.EndsWith("}")))
            {
                return Convert.ToUInt32(input.Substring(1, (input.Length - 2)), 2).ToString();
            }
            else if (input.ToLower().EndsWith("h"))
            {
                return Convert.ToUInt32(input.Substring(0, (input.Length - 1)), 16).ToString();
            }
            else if (input.ToLower().EndsWith("b"))
            {
                return Convert.ToUInt32(input.Substring(0, (input.Length - 1)), 2).ToString();
            }
            else if (input.ToLower().EndsWith("t"))
            {
                return ThornalToDecimal(input.Substring(0, (input.Length - 1)));
            }
            else if (input.StartsWith("$"))
            {
                return Convert.ToUInt32(input.Substring(1), 16).ToString();
            }
            else
            {
                return Convert.ToUInt32(input, 16).ToString();
            }
        }

        private string ThornalToDecimal(string input)
        {
            uint total = 0;
            char[] temp = input.ToCharArray();
            for (int i = input.Length - 1; i >= 0; i--)
            {
                int value = 0;
                bool success = Int32.TryParse(temp[i].ToString(), out value);
                if (!success)
                {
                    if (temp[i] < 'W' && temp[i] >= 'A')
                    {
                        value = temp[i] - 'A' + 10;
                    }
                    else
                    {
                        throw new FormatException(temp[i] + " is an invalid character in the Base 32 number set.");
                    }
                }
                total += (uint)(Math.Pow((double)32, (double)(input.Length - 1 - i)) * value);
            }
            return total.ToString();
        }

        private string ROMCharactersToString(int maxLength, uint baseLocation)
        {
            string s = "";
            for (int j = 0; j < maxLength; j++)
            {
                if ((rom[baseLocation + j] != 0xFF))
                {
                    char temp = ';';
                    bool success = characterValues.TryGetValue(rom[baseLocation + j], out temp);
                    s += temp;
                    if (!success)
                    {
                        if (rom[baseLocation + j] == 0x53)
                        {
                            s = s.Substring(0, s.Length - 1) + "PK";
                        }
                        else if (rom[baseLocation + j] == 0x54)
                        {
                            s = s.Substring(0, s.Length - 1) + "MN";
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            return s;
        }

        private Dictionary<byte, char> ReadTableFile(string iniLocation)
        {
            Dictionary<byte, char> characterValues = new Dictionary<byte, char>();
            string[] tableFile = System.IO.File.ReadAllLines(iniLocation);
            int index = 0;
            foreach (string s in tableFile)
            {
                if (!s.Equals("") && !s.Equals("[Table]") && index != 0x9E && index != 0x9F)
                {
                    string[] stuff = s.Split('=');
                    switch (Byte.Parse(ToDecimal("0x" + stuff[0])))
                    {
                        case 0:
                            characterValues.Add(0, ' ');
                            break;
                        case 0x34:
                            break;
                        case 0x35:
                            characterValues.Add(0x35, '=');
                            break;
                        case 0x53:
                            break;
                        case 0x54:
                            break;
                        case 0x55:
                            break;
                        case 0x56:
                            break;
                        case 0x57:
                            break;
                        case 0x58:
                            break;
                        case 0x59:
                            break;
                        case 0x79:
                            break;
                        case 0x7A:
                            break;
                        case 0x7B:
                            break;
                        case 0x7C:
                            break;
                        case 0xB0:
                            break;
                        case 0xEF:
                            break;
                        case 0xF7:
                            break;
                        case 0xF8:
                            break;
                        case 0xF9:
                            break;
                        case 0xFA:
                            break;
                        case 0xFB:
                            break;
                        case 0xFC:
                            break;
                        case 0xFD:
                            break;
                        case 0xFE:
                            break;
                        case 0xFF:
                            break;
                        default:
                            characterValues.Add(Byte.Parse(ToDecimal("0x" + stuff[0])), stuff[1].ToCharArray()[0]);
                            break;
                    }
                    index++;
                }
            }
            return characterValues;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.File.WriteAllBytes(chosenRom, rom);
                unsaved = false;
            }
            catch
            {
                MessageBox.Show("There was an error when quick saving.");
            }
        }

        private void lstBoxTrainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                try
                {
                    foreach (Control c in grpBoxTrainerData.Controls)
                    {
                        if (!c.Name.Equals("txtOffset") && !c.Name.Equals("chkBoxMovesets"))
                        {
                            c.Enabled = true;
                        }
                    }
                    foreach (Control c in grpBoxClass.Controls)
                    {
                        c.Enabled = true;
                    }
                    foreach (Control c in grpBoxItems.Controls)
                    {
                        c.Enabled = true;
                    }
                    foreach (Control c in grpBoxSprite.Controls)
                    {
                        c.Enabled = true;
                    }
                    foreach (Control c in grpBoxPokemon.Controls)
                    {
                        c.Enabled = true;
                    }
                    txtName.Text = ROMCharactersToString(10, (0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 4));
                    doit4 = false;
                    decimal temp2 = upDownSprite.Value;
                    upDownSprite.Value = Convert.ToDecimal(rom[(0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 3)]);
                    if (temp2 == upDownSprite.Value && firstOpen)
                    {
                        upDownSprite_ValueChanged(sender, e);
                    }
                    comboBoxClasses.SelectedIndex = rom[(0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 1)];
                    byte b = rom[classMoneyLocation];
                    decimal temp = Convert.ToDecimal((ParseWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 0x20))));
                    uint dataLocation = (ParseWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 0x24)) - 0x08000000);
                    txtOffset.Text = "0x" + dataLocation.ToString("X6");
                    upDownNumberOfPokemon.Maximum = temp;
                    upDownNumberOfPokemon.Value = temp;
                    int index = 0;
                    foundClassMoney = false;
                    while (b != 0xFF)
                    {
                        if (comboBoxClasses.SelectedIndex == b)
                        {
                            foundClassMoney = true;
                            upDownTrainerCash.Value = rom[classMoneyLocation + index + 1];
                            break;
                        }
                        index += 4;
                        b = rom[classMoneyLocation + index];
                    }
                    if (!foundClassMoney)
                    {
                        upDownTrainerCash.Value = 0;
                    }
                    lblRewardAmount.Text = "£" + CalculateNewTrainerCashReward();
                    txtClassName.Text = comboBoxClasses.SelectedItem.ToString().Split('(')[0].TrimEnd();
                    upDownAI.Value = Convert.ToDecimal(ParseWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 0x1C)));
                    upDownMusic.Value = Convert.ToDecimal(rom[(0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 2)] & 0x7F);
                    comboBoxTrainerItem1.SelectedIndex = ParseHalfWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 16));
                    comboBoxTrainerItem2.SelectedIndex = ParseHalfWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 18));
                    comboBoxTrainerItem3.SelectedIndex = ParseHalfWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 20));
                    comboBoxTrainerItem4.SelectedIndex = ParseHalfWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 22));
                    uint movesetIndex = 8;
                    if ((rom[(0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable)] & 0x1) != 0)
                    {
                        chkBoxMovesets.Checked = true;
                        movesetIndex = 16;
                    }
                    else
                    {
                        chkBoxMovesets.Checked = false;
                    }
                    if ((rom[(0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable)] & 0x2) != 0)
                    {
                        chkBoxHeldItems.Checked = true;
                    }
                    else
                    {
                        chkBoxHeldItems.Checked = false;
                    }
                    dataLength = (uint)(upDownNumberOfPokemon.Value * movesetIndex);
                    lstBoxPokemon.DataSource = null;
                    lstBoxPokemon.Items.Clear();
                    pokemonTable = new List<string>();
                    for (decimal i = 0; i < upDownNumberOfPokemon.Value; i++)
                    {
                        string s = comboBoxPokemonSpecies.Items[ParseHalfWord(dataLocation + 4 + ((uint)i * movesetIndex))].ToString() + " - Lv. " + ParseHalfWord(dataLocation + 2 + ((uint)i * movesetIndex)).ToString();
                        pokemonTable.Add(s);
                    }
                    lstBoxPokemon.DataSource = pokemonTable;
                    lstBoxPokemon.SelectedIndex = 0;
                    if ((rom[(0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 2)] & 0x80) != 0)
                    {
                        rdBtnFemale.Checked = true;
                    }
                    else
                    {
                        rdBtnMale.Checked = true;
                    }
                    doit4 = true;
                    if (chkBoxMovesets.Checked)
                    {
                        doit3 = true;
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Error parsing Pokémon data for this trainer! -\n" + exc.Message);
                }
            }
        }

        private byte ParseByte(uint baseLocation)
        {
            uint value = 0;
            ParseLoop(1, baseLocation, out value);
            return (byte)value;
        }

        private ushort ParseHalfWord(uint baseLocation)
        {
            uint value = 0;
            ParseLoop(2, baseLocation, out value);
            return (ushort)value;
        }

        private uint ParseWord(uint baseLocation)
        {
            uint value = 0;
            ParseLoop(4, baseLocation, out value);
            return value;
        }

        private void ParseLoop(uint length, uint baseLocation, out uint newValue)
        {
            newValue = 0;
            for (int i = 0; i < length; i++)
            {
                newValue += (uint)(rom[baseLocation + i] << (i * 8));
            }
        }

        private void txtSearchNumber_TextChanged(object sender, EventArgs e)
        {
            ushort trainerID = 0;
            if (!txtSearchNumber.Text.Equals(""))
            {
                UInt16.TryParse(ToDecimal("0x" + txtSearchNumber.Text), out trainerID);
                if (trainerID - 1 >= 0)
                {
                    if (trainerID - 1 <= lstBoxTrainers.Items.Count)
                    {
                        lstBoxTrainers.SelectedIndex = trainerID - 1;
                    }
                    else
                    {
                        lstBoxTrainers.SelectedIndex = 0;
                    }
                }
                else
                {
                    lstBoxTrainers.SelectedIndex = 0;
                }
            }
            else
            {
                lstBoxTrainers.SelectedIndex = 0;
            }
        }

        private void lstBoxPokemon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                foreach (Control c in grpBoxPokemonData.Controls)
                {
                    c.Enabled = true;
                }
                uint dataLocation = (ParseWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 0x24)) - 0x08000000);
                uint movesetIndex = 8;
                if (chkBoxMovesets.Checked)
                {
                    movesetIndex = 16;
                }
                doit = false;
                if (lstBoxPokemon.SelectedIndex >= 0)
                {
                    comboBoxPokemonSpecies.SelectedIndex = ParseHalfWord(dataLocation + 4 + ((uint)lstBoxPokemon.SelectedIndex * movesetIndex));
                }
                else
                {
                    comboBoxPokemonSpecies.SelectedIndex = ParseHalfWord(dataLocation + 4);
                }
                doit = true;
                txtLevel.Text = ParseHalfWord(dataLocation + 2 + ((uint)lstBoxPokemon.SelectedIndex * movesetIndex)).ToString();
                if (jambo51EVHackToolStripMenuItem.Checked)
                {
                    txtIVs.Text = ParseByte(dataLocation + ((uint)lstBoxPokemon.SelectedIndex * movesetIndex)).ToString();
                }
                else
                {
                    txtIVs.Text = ParseHalfWord(dataLocation + ((uint)lstBoxPokemon.SelectedIndex * movesetIndex)).ToString();
                }
                doit2 = false;
                if (chkBoxHeldItems.Checked)
                {
                    comboBoxHeldItem.SelectedIndex = ParseHalfWord(dataLocation + 6 + ((uint)lstBoxPokemon.SelectedIndex * movesetIndex));
                    comboBoxHeldItem.Enabled = true;
                }
                else
                {
                    comboBoxHeldItem.Enabled = false;
                    comboBoxHeldItem.SelectedIndex = 0;
                }
                doit2 = true;
                if (chkBoxMovesets.Checked)
                {
                    uint i = 0;
                    foreach (Control c in grpBoxMoveset.Controls)
                    {
                        if (c.GetType() == typeof(ComboBox))
                        {
                            c.Enabled = true;
                            doit5 = false;
                            ((ComboBox)(c)).SelectedIndex = ParseHalfWord(dataLocation + 8 + ((uint)lstBoxPokemon.SelectedIndex * movesetIndex + (2 * i)));
                            doit5 = true;
                            i++;
                        }
                    }
                }
                else
                {
                    foreach (Control c in grpBoxMoveset.Controls)
                    {
                        if (c.GetType() == typeof(ComboBox))
                        {
                            doit5 = false;
                            ((ComboBox)(c)).Enabled = false;
                            ((ComboBox)(c)).SelectedIndex = 0;
                            doit5 = true;
                        }
                    }
                }
                if (jambo51EVHackToolStripMenuItem.Checked)
                {
                    doit6 = false;
                    doit7 = false;
                    uint value = ParseByte(dataLocation + ((uint)lstBoxPokemon.SelectedIndex * movesetIndex) + 1);
                    for (int i = 0; i < 6; i++)
                    {
                        if ((value & (1 << i)) != 0)
                        {
                            SetCheckBox(true, i);
                        }
                        else
                        {
                            SetCheckBox(false, i);
                        }
                    }
                    doit7 = true;
                    comboBoxEVLevel.SelectedIndex = ((int)value & 0xC0) >> 6;
                    doit6 = true;
                }
            }
        }

        private void SetCheckBox(bool value, int index)
        {
            switch (index)
            {
                case 0:
                    chkBxEVHP.Checked = value;
                    break;
                case 1:
                    chkBxEVAttack.Checked = value;
                    break;
                case 2:
                    chkBxEVDefence.Checked = value;
                    break;
                case 3:
                    chkBxEVSpeed.Checked = value;
                    break;
                case 4:
                    chkBxEVSpecialAttack.Checked = value;
                    break;
                default:
                    chkBxEVSpecialDefence.Checked = value;
                    break;
            }
        }

        private void ToggleEVStuff()
        {
            if (!jambo51EVHackToolStripMenuItem.Checked)
            {
                lblEVs.Visible = false;
                comboBoxEVLevel.Visible = false;
                chkBxEVAttack.Visible = false;
                chkBxEVDefence.Visible = false;
                chkBxEVHP.Visible = false;
                chkBxEVSpecialAttack.Visible = false;
                chkBxEVSpecialDefence.Visible = false;
                chkBxEVSpeed.Visible = false;
                lblEVValues.Visible = false;
            }
            else
            {
                lblEVs.Visible = true;
                comboBoxEVLevel.Visible = true;
                chkBxEVAttack.Visible = true;
                chkBxEVDefence.Visible = true;
                chkBxEVHP.Visible = true;
                chkBxEVSpecialAttack.Visible = true;
                chkBxEVSpecialDefence.Visible = true;
                chkBxEVSpeed.Visible = true;
                lblEVValues.Visible = true;
            }
        }

        private void txtLevel_LostFocus(object sender, EventArgs e)
        {
            int temp = 0;
            bool success = Int32.TryParse(txtLevel.Text, out temp);
            if (!success)
            {
                txtLevel.Text = "1";
            }
            else
            {
                if (temp < 0)
                {
                    txtLevel.Text = "1";
                }
                else if (temp > 100)
                {
                    txtLevel.Text = "100";
                }
            }
            uint movesets = 8;
            if (chkBoxMovesets.Checked)
            {
                movesets = 16;
            }
            WriteToROM((ParseWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 0x24)) - 0x07FFFFFE + ((uint)lstBoxPokemon.SelectedIndex * movesets)), 2, UInt32.Parse(txtLevel.Text));
            string name = lstBoxPokemon.SelectedItem.ToString().Split('-')[0] + "- Lv. " + txtLevel.Text;
            pokemonTable[lstBoxPokemon.SelectedIndex] = name;
            CurrencyManager cm = (CurrencyManager)BindingContext[pokemonTable];
            cm.Refresh();
        }

        private void txtIVs_LostFocus(object sender, EventArgs e)
        {
            int temp = 0;
            bool success = Int32.TryParse(txtIVs.Text, out temp);
            if (!success)
            {
                txtIVs.Text = "0";
            }
            else
            {
                if (temp < 0)
                {
                    txtIVs.Text = "0";
                }
                else if (temp > 255)
                {
                    txtIVs.Text = "255";
                }
            }
            uint movesets = 8;
            if (chkBoxMovesets.Checked)
            {
                movesets = 16;
            }
            if (jambo51EVHackToolStripMenuItem.Checked)
            {
                WriteToROM((ParseWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 0x24)) - 0x08000000 + ((uint)lstBoxPokemon.SelectedIndex * movesets)), 1, UInt32.Parse(txtIVs.Text));
            }
            else
            {
                WriteToROM((ParseWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 0x24)) - 0x08000000 + ((uint)lstBoxPokemon.SelectedIndex * movesets)), 2, UInt32.Parse(txtIVs.Text));
            }
        }

        private void txtName_LostFocus(object sender, EventArgs e)
        {
            string name = (lstBoxTrainers.SelectedIndex + 1).ToString("X3") + " - " + txtName.Text;
            if (!lstBoxTrainers.SelectedItem.ToString().Equals(name))
            {
                for (uint i = 0; i < 10; i++)
                {
                    if (i < txtName.Text.Length)
                    {
                        char c = txtName.Text.Substring((int)i, 1).ToCharArray()[0];
                        if (characterValues.ContainsValue(c))
                        {
                            byte value = 0;
                            foreach (byte b in characterValues.Keys)
                            {
                                if (characterValues[b] == c)
                                {
                                    value = b;
                                    break;
                                }
                            }
                            WriteToROM((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 4 + i), 1, (uint)value);
                        }
                        else
                        {
                            WriteToROM((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 4 + i), 1, 0);
                        }
                    }
                    else if (i == txtName.Text.Length)
                    {
                        WriteToROM((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 4 + i), 1, 0xFF);
                    }
                    else
                    {
                        WriteToROM((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 4 + i), 1, 0);
                    }
                }
                table[lstBoxTrainers.SelectedIndex] = name;
                CurrencyManager cm = (CurrencyManager)BindingContext[table];
                cm.Refresh();
            }
        }

        private void comboBoxPokemonSpecies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (doit)
            {
                uint movesets = 8;
                if (chkBoxMovesets.Checked)
                {
                    movesets = 16;
                }
                WriteToROM((ParseWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 0x24)) - 0x07FFFFFC + ((uint)lstBoxPokemon.SelectedIndex * movesets)), 2, (uint)comboBoxPokemonSpecies.SelectedIndex);
                string name = comboBoxPokemonSpecies.SelectedItem.ToString();
                name += " -" + lstBoxPokemon.SelectedItem.ToString().Split('-')[1];
                pokemonTable[lstBoxPokemon.SelectedIndex] = name;
                CurrencyManager cm = (CurrencyManager)BindingContext[pokemonTable];
                cm.Refresh();
            }
        }

        public void WriteToROM(uint baselocation, uint length, uint value)
        {
            for (uint i = 0; i < length; i++)
            {
                rom[baselocation + i] = Byte.Parse(ToDecimal("0x" + value.ToString("X8").Substring(6 - (2 * (int)i), 2)));
            }
        }

        public void WriteToROM(uint baselocation, byte[] data)
        {
            for (uint i = 0; i < data.Length; i++)
            {
                rom[baselocation + i] = data[i];
            }
        }

        private void chkBoxHeldItems_CheckedChanged(object sender, EventArgs e)
        {
            ToggleEnabled(comboBoxHeldItem);
            uint location = (0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable);
            if (comboBoxHeldItem.Enabled)
            {
                WriteToROM(location, 1, ((uint)(rom[location] | 2)));
            }
            else
            {
                WriteToROM(location, 1, ((uint)(rom[location] & 0xFD)));
            }
        }

        private void ToggleEnabled(Control c)
        {
            if (c.Enabled)
            {
                c.Enabled = false;
                return;
            }
            c.Enabled = true;
        }

        private void comboBoxHeldItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (doit2)
            {
                uint movesets = 8;
                if (chkBoxMovesets.Checked)
                {
                    movesets = 16;
                }
                WriteToROM((ParseWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 0x24)) - 0x07FFFFFA + ((uint)lstBoxPokemon.SelectedIndex * movesets)), 2, (uint)comboBoxHeldItem.SelectedIndex);
            }
        }

        private void comboBoxKnownMoves1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (doit3)
            {
                HandleMoveChange(((ComboBox)(sender)));
            }
        }

        private void HandleMoveChange(ComboBox c)
        {
            if (doit5)
            {
                uint moveIndex = (uint)((UInt32.Parse(c.Name.Substring(c.Name.Length - 1)) - (uint)1) << 1);
                WriteToROM((ParseWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 0x24)) - 0x07FFFFF8 + moveIndex + ((uint)lstBoxPokemon.SelectedIndex * 16)), 2, (uint)c.SelectedIndex);
            }
        }

        private void btnChangeNumberOfPokemon_Click(object sender, EventArgs e)
        {
            Data_Repoint_Form frm = new Data_Repoint_Form(dataLength, chkBoxMovesets.Checked, rom, UInt32.Parse(ToDecimal(txtOffset.Text)), this, (0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable));
            frm.ShowDialog(this);
        }

        private void upDownAI_ValueChanged(object sender, EventArgs e)
        {
            if (doit4)
            {
                if ((upDownAI.Value >= 0 && upDownAI.Value < 8) || upDownAI.Value == 28)
                {
                    WriteToROM((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 0x1C), 4, (uint)upDownAI.Value);
                }
            }
            if ((upDownAI.Value >= 0 && upDownAI.Value < 8) || upDownAI.Value == 28)
            {
                lblExtraInfo.Visible = false;
            }
            else
            {
                lblExtraInfo.Visible = true;
            }
        }

        private void upDownMusic_ValueChanged(object sender, EventArgs e)
        {
            if (doit4)
            {
                WriteToROM((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 2), 1, (uint)((rom[(0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 2)] & 0x80) | (int)upDownMusic.Value));
            }
        }

        private void rdBtnMale_CheckedChanged(object sender, EventArgs e)
        {
            if (doit4)
            {
                if (rdBtnMale.Checked)
                {
                    WriteToROM((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 2), 1, (uint)(rom[(0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 2)] & 0x7F));
                }
                else
                {
                    WriteToROM((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 2), 1, (uint)((rom[(0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 2)] & 0x7F) | 0x80));
                }
            }
        }

        private void comboBoxClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (doit4)
            {
                WriteToROM(0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 1, 1, (uint)comboBoxClasses.SelectedIndex);
                txtClassName.Text = comboBoxClasses.SelectedItem.ToString().Split('(')[0].TrimEnd();
            }
        }

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            ushort tempVal = 0;
            UInt16.TryParse(ToDecimal("0x" + txtSearchNumber.Text), out tempVal);
            if (tempVal >= lstBoxTrainers.Items.Count)
            {
                tempVal = 0;
            }
            for (int i = tempVal; i < lstBoxTrainers.Items.Count; i++)
            {
                if (nameLookUp[i].ToLower().StartsWith(txtSearchName.Text))
                {
                    lstBoxTrainers.SelectedIndex = i;
                    return;
                }
            }
            lstBoxTrainers.SelectedIndex = 0;
        }

        private void btnChangeClass_Click(object sender, EventArgs e)
        {
            string name = "";
            uint indexShift = 0;
            bool FFwritten = false;
            for (uint i = 0; i < 13; i++)
            {
                if (i == 12 && !FFwritten)
                {
                    WriteToROM(((uint)(0xD * comboBoxClasses.SelectedIndex + classNamesLocation + i - indexShift)), 1, 0xFF);
                }
                else if (i < txtClassName.Text.Length)
                {
                    char c = txtClassName.Text.Substring((int)i, 1).ToCharArray()[0];
                    if (c == 'P')
                    {
                        try
                        {
                            char c2 = txtClassName.Text.Substring((int)i + 1, 1).ToCharArray()[0];
                            if (c2 == 'K')
                            {
                                WriteToROM(((uint)(0xD * comboBoxClasses.SelectedIndex + classNamesLocation + i - indexShift)), 1, 0x53);
                                i++;
                                indexShift++;
                                name += "PK";
                                continue;
                            }
                        }
                        catch
                        {
                        }
                    }
                    else if (c == 'M')
                    {
                        try
                        {
                            char c2 = txtClassName.Text.Substring((int)i + 1, 1).ToCharArray()[0];
                            if (c2 == 'N')
                            {
                                WriteToROM(((uint)(0xD * comboBoxClasses.SelectedIndex + classNamesLocation + i - indexShift)), 1, 0x54);
                                i++;
                                indexShift++;
                                name += "MN";
                                continue;
                            }
                        }
                        catch
                        {
                        }
                    }
                    if (characterValues.ContainsValue(c))
                    {
                        byte value = 0;
                        foreach (byte b in characterValues.Keys)
                        {
                            if (characterValues[b] == c)
                            {
                                value = b;
                                break;
                            }
                        }
                        WriteToROM(((uint)(0xD * comboBoxClasses.SelectedIndex + classNamesLocation + i - indexShift)), 1, (uint)value);
                        name += c;
                    }
                    else
                    {
                        WriteToROM(((uint)(0xD * comboBoxClasses.SelectedIndex + classNamesLocation + i - indexShift)), 1, 0);
                        name += ' ';
                    }
                }
                else if (i == txtClassName.Text.Length)
                {
                    WriteToROM(((uint)(0xD * comboBoxClasses.SelectedIndex + classNamesLocation + i - indexShift)), 1, 0xFF);
                    FFwritten = true;
                }
                else
                {
                    WriteToROM(((uint)(0xD * comboBoxClasses.SelectedIndex + classNamesLocation + i - indexShift)), 1, 0);
                }
            }
            classesTable[comboBoxClasses.SelectedIndex] = name + " (0x" + comboBoxClasses.SelectedIndex.ToString("X") + ")";
            CurrencyManager cm = (CurrencyManager)BindingContext[classesTable];
            cm.Refresh();
        }

        private void upDownSprite_ValueChanged(object sender, EventArgs e)
        {
            if (doit4)
            {
                WriteToROM((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 3), 1, (uint)upDownSprite.Value);
            }
            LZ77Handler temp = new LZ77Handler();
            picBoxTrainer.Image = temp.LoadSprite((int)ParseWord((uint)(((int)upDownSprite.Value << 3) + (int)trainerImageTable)) - 0x08000000, (int)ParseWord((uint)(((int)upDownSprite.Value << 3) + (int)trainerPaletteTable)) - 0x08000000, rom, showBackgroundColours);
            picBoxTrainer.Refresh();
        }

        public void UpdateTrainerData()
        {
            lstBoxTrainers_SelectedIndexChanged(new object(), new EventArgs());
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (romOpen)
            {
                DialogResult result = saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    try
                    {
                        System.IO.File.WriteAllBytes(saveFileDialog1.FileName, rom);
                        unsaved = false;
                    }
                    catch
                    {
                        MessageBox.Show("There was en error saving the ROM");
                    }
                }
            }
            else
            {
                MessageBox.Show("No ROM Open. Cannot save!");
            }
        }

        private void jambo51EVHackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleEVStuff();
            try
            {
                string location = System.Windows.Forms.Application.StartupPath + @"\TrainerEditor.ini";
                string[] file = System.IO.File.ReadAllLines(location);
                List<string> newFile = new List<string>();
                foreach (string s in file)
                {
                    if (s.StartsWith("EVHack"))
                    {
                        newFile.Add("EVHack=" + jambo51EVHackToolStripMenuItem.Checked.ToString());
                    }
                    else
                    {
                        newFile.Add(s);
                    }
                }
                System.IO.File.WriteAllLines(location, newFile.ToArray());
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("Could not find the ini file!");
            }
            catch (Exception exc)
            {
                MessageBox.Show("There was an unknown error.\n" + exc.Message);
            }
        }

        private void comboBoxEVLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            float value = 0;
            if (numberOfEVsActive == 0 && comboBoxEVLevel.SelectedIndex != 0)
            {
                value = (510.0f * (float)(comboBoxEVLevel.SelectedIndex / 3.0f));
                if (value > 0xFF)
                {
                    value = 255.0f;
                }
            }
            else if (numberOfEVsActive == 0)
            {
                value = 0;
            }
            else
            {
                value = (float)(510.0f / numberOfEVsActive) * (float)(comboBoxEVLevel.SelectedIndex / 3.0f);
                if (value > 0xFF)
                {
                    value = 255.0f;
                }
            }
            lblEVValues.Text = "Each EV Will Receive: " + ((uint)(value)).ToString();
            if (doit6)
            {
                uint movesets = 8;
                if (chkBoxMovesets.Checked)
                {
                    movesets = 16;
                }
                uint location = (ParseWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 0x24)) - 0x08000000 + ((uint)lstBoxPokemon.SelectedIndex * movesets) + 1);
                WriteToROM(location, 1, (uint)((rom[location] & 0x3F) | (comboBoxEVLevel.SelectedIndex << 6)));
            }
        }

        private void chkBxEVHP_CheckedChanged(object sender, EventArgs e)
        {
            RaiseLowerEVCounter(sender, 1);
        }

        private void RaiseLowerEVCounter(object sender, int value)
        {
            if (((CheckBox)(sender)).Checked)
            {
                numberOfEVsActive++;
            }
            else
            {
                numberOfEVsActive--;
            }
            if (doit7)
            {
                uint movesets = 8;
                if (chkBoxMovesets.Checked)
                {
                    movesets = 16;
                }
                uint location = (ParseWord((0x28 * (uint)lstBoxTrainers.SelectedIndex + trainerTable + 0x24)) - 0x08000000 + ((uint)lstBoxPokemon.SelectedIndex * movesets) + 1);
                if (((CheckBox)(sender)).Checked)
                {
                    WriteToROM(location, 1, (uint)(rom[location] | value));
                }
                else
                {
                    WriteToROM(location, 1, (uint)(rom[location] & (~(value))));
                }
                doit6 = false;
                comboBoxEVLevel_SelectedIndexChanged(sender, new EventArgs());
                doit6 = true;
            }
        }

        private void chkBxEVAttack_CheckedChanged(object sender, EventArgs e)
        {
            RaiseLowerEVCounter(sender, 2);
        }

        private void chkBxEVDefence_CheckedChanged(object sender, EventArgs e)
        {
            RaiseLowerEVCounter(sender, 4);
        }

        private void chkBxEVSpeed_CheckedChanged(object sender, EventArgs e)
        {
            RaiseLowerEVCounter(sender, 8);
        }

        private void chkBxEVSpecialAttack_CheckedChanged(object sender, EventArgs e)
        {
            RaiseLowerEVCounter(sender, 16);
        }

        private void chkBxEVSpecialDefence_CheckedChanged(object sender, EventArgs e)
        {
            RaiseLowerEVCounter(sender, 32);
        }

        private void upDownTrainerCash_ValueChanged(object sender, EventArgs e)
        {
            if (foundClassMoney && doit4)
            {
                byte b = (byte)comboBoxClasses.SelectedIndex;
                byte c = rom[classMoneyLocation];
                uint index = 0;
                while (c != b)
                {
                    index += 4;
                    c = rom[classMoneyLocation + index];
                }
                WriteToROM(classMoneyLocation + index + 1, 1, (uint)upDownTrainerCash.Value);
                lblRewardAmount.Text = "£" + CalculateNewTrainerCashReward();
            }
            else if (foundClassMoney)
            {
                lblRewardAmount.Text = "£" + CalculateNewTrainerCashReward();
            }
            else
            {
                lblRewardAmount.Text = "£0";
            }
        }

        private string CalculateNewTrainerCashReward()
        {
            uint movesets = 8;
            if (chkBoxMovesets.Checked)
            {
                movesets = 16;
            }
            return (upDownTrainerCash.Value * (uint)(ParseByte((uint)upDownNumberOfPokemon.Value * movesets + UInt32.Parse(ToDecimal(txtOffset.Text)) - movesets + 2) << 2)).ToString();
        }

        private void upDownNumberOfPokemon_ValueChanged(object sender, EventArgs e)
        {
            upDownTrainerCash_ValueChanged(sender, e);
        }
    }
}
