using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FGOtest
{
    public partial class FormSelectRW : Form
    {
        Form1 formMain;

        Button[] colorCard;
        GroupBox[] skillBox;
        Label[] CDlabel;
        Label[] effectNumLabel;
        TextBox[] CDText;
        NumericUpDown[] effectNum;
        ComboBox[] effectBox;
        TextBox[] effectText;
        Label[] effectlabel;

        TextBox[] afterEffectText;

        const int effectMax = 6;//最多效果数
        const int skillNum= 3;//012主动技能 3被动技能 4宝具 5礼装
        const int comboNum = 2;//combo几行
        const int textNum = 3;//text几行

        public FormSelectRW(Form1 formInit)
        {
            formMain = formInit;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(200 + 180 * effectMax, 600);

            InitializeComponent();

            setComponent();

            formMain.memory.refreshComboBox(servantComboBox);
            formMain.memory.showComboBox(servantComboBox, 2);
            
            formMain.memory.refreshCEComboBox(CraftEssenceComboBox);
        }

        private void setComponent()//初始化控件
        {
            for (int i = 0; i < formMain.className.Length; i++)//初始化职介下拉栏
            {
                string temp = formMain.className[i];
                classComboBox.Items.Add(temp);
            }
            for (int i = 0; i < formMain.attributeName.Length; i++)//初始化阵营下拉栏
            {
                string temp = formMain.attributeName[i];
                attributeComboBox.Items.Add(temp);
            }
            for (int i = 1; i < formMain.cardColorName.Length; i++)//初始化宝具颜色下拉栏
            {
                string temp = formMain.cardColorName[i];
                NPtypeComboBox.Items.Add(temp);
            }

            //初始化行动卡设置
            colorCard = new Button[5];
            for (int i = 0; i < 5; i++)
            {
                colorCard[i] = new Button();
                colorCard[i].Name = "colorCard" + i.ToString();
                colorCard[i].Location = new System.Drawing.Point(50 + 75 * i, 250);
                colorCard[i].Size = new System.Drawing.Size(50, 50);
                colorCard[i].Click += new System.EventHandler(this.colorCard_Click);
                this.Controls.Add(colorCard[i]);
            }

            //初始化所有groupbox 0-2主动技能 3被动技能 4宝具 5礼装
            skillBox = new GroupBox[skillNum+3];
            for (int i = 0; i < skillNum+1; i++)
            {
                skillBox[i] = new GroupBox();
                skillBox[i].Name = "skillBox" + i.ToString();
                skillBox[i].Location = new System.Drawing.Point(120 + 90 * effectMax, 20 + 220 * i);
                skillBox[i].Size = new System.Drawing.Size(60 + 90 * effectMax, 200);
                if(i==skillNum+1-1) skillBox[i].Size = new System.Drawing.Size(60 + 90 * effectMax, 135);
                skillBox[i].Text = "主动技能" + (i + 1).ToString();
                this.Controls.Add(skillBox[i]);
            }
            skillBox[skillNum].Text = "被动技能";
            skillBox[skillNum+1] = NPgroupBox;
            skillBox[skillNum+1].Size= new System.Drawing.Size(60 + 90 * effectMax, 260);
            skillBox[skillNum+2] = CEgroupBox;
            skillBox[skillNum+2].Size = new System.Drawing.Size(60 + 90 * effectMax, 240);

            //初始化CD框
            CDlabel = new Label[skillNum];
            for (int i = 0; i < skillNum; i++)
            {
                CDlabel[i] = new Label();
                CDlabel[i].Name = "CDlabel" + i.ToString();
                CDlabel[i].Location = new System.Drawing.Point(50, 20);
                CDlabel[i].Size = new System.Drawing.Size(30, 30);
                CDlabel[i].Text = "CD";
                skillBox[i].Controls.Add(CDlabel[i]);
            }
            CDText = new TextBox[skillNum+3];
            for (int i = 0; i < skillNum+3; i++)
            {
                CDText[i] = new TextBox();
                CDText[i].Name = "CDText" + i.ToString();
                CDText[i].Location = new System.Drawing.Point(80, 20);
                CDText[i].Size = new System.Drawing.Size(50, 20);
                skillBox[i].Controls.Add(CDText[i]);
                if (i >= skillNum) CDText[i].Visible = false;
            }

            //初始化效果数量选择框
            effectNumLabel = new Label[skillNum+3];
            for (int i = 0; i < skillNum+3; i++)
            {
                effectNumLabel[i] = new Label();
                effectNumLabel[i].Name = "effectNumLabel" + i.ToString();
                effectNumLabel[i].Location = new System.Drawing.Point(150, 20);
                effectNumLabel[i].Size = new System.Drawing.Size(50, 20);
                effectNumLabel[i].Text = "效果数";
                skillBox[i].Controls.Add(effectNumLabel[i]);
            }
            effectNum = new NumericUpDown[skillNum+3];
            for (int i = 0; i < skillNum+3; i++)
            {
                effectNum[i] = new NumericUpDown();
                effectNum[i].Name = "effectNum" + i.ToString();
                effectNum[i].Location = new System.Drawing.Point(200, 20);
                effectNum[i].Size = new System.Drawing.Size(50, 20);
                effectNum[i].Value = effectMax;
                effectNum[i].Maximum = effectMax;
                effectNum[i].Minimum = 0;
                effectNum[i].ValueChanged += new System.EventHandler(this.effectNum_Changed);
                skillBox[i].Controls.Add(effectNum[i]);
            }

            //初始化所有效果框
            effectBox = new ComboBox[(skillNum+3) * comboNum * effectMax];
            for (int i = 0; i < (skillNum+3) * comboNum * effectMax; i++)
            {
                effectBox[i] = new ComboBox();
                effectBox[i].Name = "effectBox" + i.ToString();
                effectBox[i].Location = new System.Drawing.Point(50 + 90 * ((i % (comboNum * effectMax)) / comboNum), 50 + 30 * ((i % (comboNum * effectMax)) % comboNum));
                effectBox[i].Size = new System.Drawing.Size(80, 20);
                effectBox[i].DropDownStyle = ComboBoxStyle.DropDownList;
                effectBox[i].DropDownHeight = 200;
                if (i % 2 == 0)
                {
                    for (int j = 0; j < formMain.targetTypeName.Length; j++)
                    {
                        effectBox[i].Items.Add(formMain.targetTypeName[j]);
                    }
                }
                else
                {
                    for (int j = 0; j < formMain.effectName.Length; j++)
                    {
                        effectBox[i].Items.Add(formMain.effectName[j]);
                    }
                }
                skillBox[i / (comboNum * effectMax)].Controls.Add(effectBox[i]);
            }
            effectText = new TextBox[(skillNum+3) * textNum * effectMax];
            for (int i = 0; i < (skillNum+3) * textNum * effectMax; i++)
            {
                effectText[i] = new TextBox();
                effectText[i].Name = "effectText" + i.ToString();
                effectText[i].Location = new System.Drawing.Point(50 + 90 * ((i % (textNum * effectMax)) / textNum), 110 + 30 * ((i % (textNum * effectMax)) % textNum));
                effectText[i].Size = new System.Drawing.Size(80, 20);
                effectText[i].Text = "0";
                if (i % 3 == 1) effectText[i].Text = "-1";
                if (i % 3 == 2) effectText[i].Text = "1";
                skillBox[i / (textNum * effectMax)].Controls.Add(effectText[i]);
            }
            effectlabel = new Label[(skillNum+3) * 5 +1];
            for (int i = 0; i < (skillNum+3) * 5; i++)
            {
                effectlabel[i] = new Label();
                effectlabel[i].Name = "effectlabel" + i.ToString();
                effectlabel[i].Location = new System.Drawing.Point(10, 50 + 30 * (i % 5));
                effectlabel[i].Size = new System.Drawing.Size(50, 20);
                if (i % 5 == 0) effectlabel[i].Text = "目标";
                if (i % 5 == 1) effectlabel[i].Text = "类别";
                if (i % 5 == 2) effectlabel[i].Text = "数值";
                if (i % 5 == 3) effectlabel[i].Text = "回合";
                if (i % 5 == 4) effectlabel[i].Text = "概率";
                if(i%5==4 && i/5==skillNum+2) effectlabel[i].Text = "满破值";
                skillBox[i / 5].Controls.Add(effectlabel[i]);
            }
            effectlabel[(skillNum + 3) * 5] = new Label();
            effectlabel[(skillNum + 3) * 5].Name = "effectlabel" + ((skillNum + 3) * 5).ToString();
            effectlabel[(skillNum + 3) * 5].Location = new System.Drawing.Point(10, 200);
            effectlabel[(skillNum + 3) * 5].Size = new System.Drawing.Size(40, 20);
            effectlabel[(skillNum + 3) * 5].Text = "后效";
            skillBox[skillNum+1].Controls.Add(effectlabel[(skillNum + 3) * 5]);

            afterEffectText = new TextBox[effectMax];//宝具独有后发动效果框
            for (int i = 0; i < effectMax; i++)
            {
                afterEffectText[i] = new TextBox();
                afterEffectText[i].Name = "afterEffectText" + i.ToString();
                afterEffectText[i].Location = new System.Drawing.Point(50 + 90 * i, 200);
                afterEffectText[i].Size = new System.Drawing.Size(80, 20);
                afterEffectText[i].Text = "0";
                skillBox[skillNum+1].Controls.Add(afterEffectText[i]);
            }
        }

        private void effectNum_Changed(object sender, System.EventArgs e)//根据效果数量改变所有效果框Enabled
        {
            int skillNo = formMain.getTNoFromName(((NumericUpDown)sender).Name);
            int effectNum = (int)((NumericUpDown)sender).Value;
            for (int i = 0; i < comboNum * effectMax; i++)
            {
                effectBox[skillNo * comboNum * effectMax + i].Enabled = i < comboNum * effectNum;
            }
            for (int i = 0; i < textNum * effectMax; i++)
            {
                effectText[skillNo * textNum * effectMax + i].Enabled = i < textNum * effectNum;
            }
            if (skillNo == skillNum + 1)
            {
                for (int i = 0; i < effectMax; i++)
                {
                    afterEffectText[i].Enabled = i < effectNum;
                }
            }
        }        

        private void colorCard_Click(object sender, System.EventArgs e)//点击循环改变行动卡卡色
        {
            if (((Button)sender).BackColor == Color.Red)
            {
                ((Button)sender).BackColor = Color.Blue;
            }
            else if (((Button)sender).BackColor == Color.Blue)
            {
                ((Button)sender).BackColor = Color.Green;
            }
            else
            {
                ((Button)sender).BackColor = Color.Red;
            }
        }

        private Servant readServantFromTable()//将界面从者数据读取进内存
        {
            Servant tempServant = new Servant();

            tempServant.ID = int.Parse(IDText.Text);
            tempServant.name = nameText.Text;
            tempServant.rarity = int.Parse(rarityText.Text);
            tempServant.servantClass = formMain.getNoFromName(formMain.className, classComboBox.Text);
            tempServant.cost = int.Parse(costText.Text);
            tempServant.ATK = int.Parse(ATKText.Text);
            tempServant.HP = int.Parse(HPText.Text);
            tempServant.attribute = formMain.getNoFromName(formMain.attributeName, attributeComboBox.Text);
            tempServant.npCharge = double.Parse(npCText.Text);
            tempServant.npChargeDEF = double.Parse(npCdText.Text);
            tempServant.starGe = double.Parse(starGText.Text);
            tempServant.starAb = int.Parse(starAText.Text);
            tempServant.redHits = int.Parse(colorRHText.Text);
            tempServant.blueHits = int.Parse(colorBHText.Text);
            tempServant.greenHits = int.Parse(colorGHText.Text);
            tempServant.EXHits = int.Parse(EXHText.Text);
            tempServant.redCards = 0;
            tempServant.blueCards = 0;
            tempServant.greenCards = 0;
            for (int i = 0; i < 5; i++)
            {
                if (colorCard[i].BackColor == Color.Red) tempServant.redCards++;
                if (colorCard[i].BackColor == Color.Blue) tempServant.blueCards++;
                if (colorCard[i].BackColor == Color.Green) tempServant.greenCards++;
            }
            tempServant.NPtype = formMain.getNoFromName(formMain.cardColorName, NPtypeComboBox.Text);
            tempServant.NPdmg = double.Parse(NPdmgText.Text);
            tempServant.NPHits = int.Parse(NPHitsText.Text);
            tempServant.NPtarget = int.Parse(NPtargetText.Text);

            tempServant.skills[skillNum+1].CD = 0;
            int NPeffNum = (int)effectNum[skillNum+1].Value;
            tempServant.skills[skillNum+1].clearEffect();
            for (int i = 0; i < NPeffNum; i++)
            {
                int targetT = formMain.getNoFromName(formMain.targetTypeName, effectBox[comboNum * effectMax * (skillNum+1)+comboNum * i + 0].Text);
                int type = formMain.getNoFromName(formMain.effectName, effectBox[comboNum * effectMax * (skillNum + 1) + comboNum * i + 1].Text);
                double value = double.Parse(effectText[textNum * effectMax * (skillNum + 1) + textNum * i + 0].Text);
                int turn = int.Parse(effectText[textNum * effectMax * (skillNum + 1) + textNum * i + 1].Text);
                double probability = double.Parse(effectText[textNum * effectMax * (skillNum + 1) + textNum * i + 2].Text);
                bool afterNP = (afterEffectText[i].Text == "1") ? true : false;
                tempServant.skills[skillNum+1].addEffect(targetT, type, value, turn, probability, afterNP);
            }

            for (int j = 0; j < skillNum+1; j++)
            {
                tempServant.skills[j].CD = int.Parse(CDText[j].Text);
                int effNum = (int)effectNum[j].Value;
                tempServant.skills[j].clearEffect();
                for (int i = 0; i < effNum; i++)
                {
                    int targetT = formMain.getNoFromName(formMain.targetTypeName, effectBox[comboNum * effectMax * j + comboNum * i + 0].Text);
                    int type = formMain.getNoFromName(formMain.effectName, effectBox[comboNum * effectMax * j + comboNum * i + 1].Text);
                    double value = double.Parse(effectText[textNum * effectMax * j + textNum * i + 0].Text);
                    int turn = int.Parse(effectText[textNum * effectMax * j + textNum * i + 1].Text);
                    double probability = double.Parse(effectText[textNum * effectMax * j + textNum * i + 2].Text);
                    tempServant.skills[j].addEffect(targetT, type, value, turn, probability, false);
                }
            }

            return tempServant;
        }
        private void servantComboBox_SelectedIndexChanged(object sender, EventArgs e)//从已有列表选择一个从者
        {
            Servant servant = formMain.memory.getServantWithComboText(((ComboBox)sender).SelectedItem.ToString());

            IDText.Text = servant.ID.ToString();
            nameText.Text = servant.name;
            rarityText.Text = servant.rarity.ToString();
            classComboBox.Text = formMain.className[servant.servantClass];
            costText.Text = servant.cost.ToString();

            ATKText.Text = servant.ATK.ToString();
            HPText.Text = servant.HP.ToString();
            attributeComboBox.Text = formMain.attributeName[servant.attribute];
            npCText.Text = servant.npCharge.ToString();
            npCdText.Text = servant.npChargeDEF.ToString();
            starGText.Text = servant.starGe.ToString();
            starAText.Text = servant.starAb.ToString();

            colorRHText.Text = servant.redHits.ToString();
            colorBHText.Text = servant.blueHits.ToString();
            colorGHText.Text = servant.greenHits.ToString();
            EXHText.Text = servant.EXHits.ToString();
            for (int i = 0; i < 5; i++)
            {
                if (i < servant.redCards)
                {
                    colorCard[i].BackColor = Color.Red;
                }
                else if (i < servant.redCards + servant.blueCards)
                {
                    colorCard[i].BackColor = Color.Blue;
                }
                else
                {
                    colorCard[i].BackColor = Color.Green;
                }

            }

            NPtypeComboBox.Text = formMain.cardColorName[servant.NPtype];
            NPdmgText.Text = servant.NPdmg.ToString();
            NPHitsText.Text = servant.NPHits.ToString();
            NPtargetText.Text = servant.NPtarget.ToString();

            effectNum[skillNum + 1].Value = servant.skills[skillNum + 1].getEffectNum();
            for (int i = 0; i < effectNum[skillNum + 1].Value; i++)
            {
                SingleEffect tempEff = servant.skills[skillNum + 1].getEffect(i);
                effectBox[comboNum * effectMax * (skillNum + 1) + comboNum * i + 0].Text = formMain.targetTypeName[tempEff.targetType];
                effectBox[comboNum * effectMax * (skillNum + 1) + comboNum * i + 1].Text = formMain.effectName[tempEff.type];
                effectText[textNum * effectMax * (skillNum + 1) + textNum * i + 0].Text = tempEff.value.ToString();
                effectText[textNum * effectMax * (skillNum + 1) + textNum * i + 1].Text = tempEff.turn.ToString();
                effectText[textNum * effectMax * (skillNum + 1) + textNum * i + 2].Text = tempEff.probability.ToString();
                afterEffectText[i].Text = (tempEff.afterNP == true) ? "1" : "0";
            }

            for (int i = 0; i < skillNum + 1; i++)
            {
                CDText[i].Text = servant.skills[i].CD.ToString();
                effectNum[i].Value = servant.skills[i].getEffectNum();
                for (int j = 0; j < effectNum[i].Value; j++)
                {
                    SingleEffect tempEff = servant.skills[i].getEffect(j);
                    effectBox[comboNum * effectMax * i + comboNum * j + 0].Text = formMain.targetTypeName[tempEff.targetType];
                    effectBox[comboNum * effectMax * i + comboNum * j + 1].Text = formMain.effectName[tempEff.type];
                    effectText[textNum * effectMax * i + textNum * j + 0].Text = tempEff.value.ToString();
                    effectText[textNum * effectMax * i + textNum * j + 1].Text = tempEff.turn.ToString();
                    effectText[textNum * effectMax * i + textNum * j + 2].Text = tempEff.probability.ToString();
                }
            }
        }
        private void writeButton_Click(object sender, EventArgs e)//将读取的从者写入文件
        {
            Servant writeServant = readServantFromTable();
            formMain.memory.addServant(writeServant);

            formMain.memory.refreshComboBox(servantComboBox);
            formMain.memory.showComboBox(servantComboBox, writeServant.ID);
            MessageBox.Show("更新成功");
        }

        private CraftEssence readCEFromTable()//将界面礼装数据读取进内存
        {
            CraftEssence tempCE = new CraftEssence();

            tempCE.ID = int.Parse(CEIDText.Text);
            string[] t = CraftEssenceComboBox.Text.Split(' ');
            if (t.Length > 1)
                tempCE.name = t[1];
            else
                tempCE.name = t[0];
            tempCE.rarity = int.Parse(CERarityText.Text);
            tempCE.cost = int.Parse(CECostText.Text);
            tempCE.ATK = int.Parse(CEATKText.Text);
            tempCE.HP = int.Parse(CEHPText.Text);


            int effNum = (int)effectNum[skillNum + 2].Value;
            for (int i = 0; i < effNum; i++)
            {
                int targetT = formMain.getNoFromName(formMain.targetTypeName, effectBox[comboNum * effectMax * (skillNum + 2) + comboNum * i + 0].Text);
                int type = formMain.getNoFromName(formMain.effectName, effectBox[comboNum * effectMax * (skillNum + 2) + comboNum * i + 1].Text);
                double value = double.Parse(effectText[textNum * effectMax * (skillNum + 2) + textNum * i + 0].Text);
                int turn = int.Parse(effectText[textNum * effectMax * (skillNum + 2) + textNum * i + 1].Text);
                double fullvalue = double.Parse(effectText[textNum * effectMax * (skillNum + 2) + textNum * i + 2].Text);
                tempCE.addEffect(targetT, type, value, turn, 1.0, false, fullvalue);
            }

            return tempCE;
        }
        private void CraftEssenceComboBox_SelectedIndexChanged(object sender, EventArgs e)//从已有列表选择一个礼装
        {

            CraftEssence ce = formMain.memory.getCEWithComboText(((ComboBox)sender).SelectedItem.ToString());
            if (ce == null) return;

            CEIDText.Text = ce.ID.ToString();
            CERarityText.Text = ce.rarity.ToString();
            CECostText.Text = ce.cost.ToString();
            CEATKText.Text = ce.ATK.ToString();
            CEHPText.Text = ce.HP.ToString();


            effectNum[skillNum + 2].Value = ce.getEffectNum();
            for (int i = 0; i < effectNum[skillNum + 2].Value; i++)
            {
                SingleEffect tempEff = ce.getEffect(i);
                effectBox[comboNum * effectMax * (skillNum + 2) + comboNum * i + 0].Text = formMain.targetTypeName[tempEff.targetType];
                effectBox[comboNum * effectMax * (skillNum + 2) + comboNum * i + 1].Text = formMain.effectName[tempEff.type];
                effectText[textNum * effectMax * (skillNum + 2) + textNum * i + 0].Text = tempEff.value.ToString();
                effectText[textNum * effectMax * (skillNum + 2) + textNum * i + 1].Text = tempEff.turn.ToString();
                effectText[textNum * effectMax * (skillNum + 2) + textNum * i + 2].Text = tempEff.fullvalue.ToString();
            }
            
        }
        private void CEwritebutton_Click(object sender, EventArgs e)//将读取的礼装写入文件
        {
            CraftEssence writeCE = readCEFromTable();
            formMain.memory.addCE(writeCE);

            formMain.memory.refreshCEComboBox(CraftEssenceComboBox);
            MessageBox.Show("更新成功");
        }
    }
}

