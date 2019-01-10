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
    public partial class Form1 : Form
    {
        public Memory memory;
        private Battlefield battlefield;

        FormSelectRW formRW;

        GroupBox[] servantBox;//3个从者box
        GroupBox[] someBox;//0结果和1环境box

        //从者box里数据部分
        ComboBox[] servantComboList;//3个我方从者下拉框
        ComboBox[] CEComboList;//3个礼装下拉框
        CheckBox[] CEMLCheck;//3个礼装满破检查框
        TextBox[] ATKText;
        TextBox[] HPText;
        TextBox[] NPText;
        TextBox[] sumNPText;
        TextBox[] ADMGText;
        TextBox[] NPDMGText;
        Label[] servantLabel;

        //从者box里AI部分
        Button[] skillButton;//3*3技能框
        Button[] NPButton;//3个宝具按钮
        NumericUpDown[] NPNumeric;//3个宝具等级
        ComboBox[] strategyComboList;//18,6个为一角色,0-2为释放对象,3-5为释放策略
        Button[] colorButton;//4*4颜色卡按钮，前3组为0-2对象，最后一组为环境
        TextBox[] priorityText;//3*3+4行动卡优先级，前三组为0-2行动卡，最后一组为环境chain优先级
        TextBox[] NPpriorityText;//3个宝具优先级
        CheckBox[] specialCheck;//3个特攻检查框
        Button[] checkBuffButton;//4个buff查看按钮，0-2为我方，3为敌方
        Label[] servantAILabel;

        //结果box
        Button[] resultButton;//5+3+4 5候选卡，3宝具卡，4结果卡
        TextBox[] resultText;//4个结果卡结果
        Label[] resultLabel;

        //环境box
        Label[] enviLabel;
        NumericUpDown[] enviNumeric;//0我方从者数量1敌方从者数量
        Button[] enviButton;//修改和更新按钮
        ComboBox enemyServantClassCombo;//控制敌方阶级

        //各类文本
        public string[] cardColorName = new string[4] { "白", "红", "蓝", "绿" };//输入数字可得到颜色的常量
        public string[] className = new string[13] { "shielder", "saber", "archer", "lancer", "rider", "caster", "assassin", "berserker", "ruler", "avenger", "alter ego", "moon cancer", "foreigner" };////输入数字可得到职介的常量
        public double[] classATKbonusList = new double[13] { 1.0, 1.0, 0.95, 1.05, 1.0, 0.9, 0.9, 1.1, 1.1, 1.1, 1.0, 1.0, 1.0 };
        public string[] attributeName = new string[5] { "Man", "Sky", "Earth", "Star", "Beast" };
        public string[] effectName = new string[30] { "无", "攻击力", "np率", "出星率", "集星", "宝具威力", "红魔放", "蓝魔放", "绿魔放"
            , "爆伤", "弱体耐性", "防御力", "额外伤害", "获得np", "出星", "回复生命", "战续", "闪避", "无敌", "嘲讽", "眩晕", "蓄力"
            ,"减CD" ,"特攻","宝具特攻","无视闪避","无敌贯通","弱体成功","持续伤害","强化成功"};//持续伤害没写
        public string[] targetTypeName = new string[8] { "无", "自身", "队友单体", "队友全体", "除自身外全体", "除单体外全体", "敌方单体", "敌方全体" };
        public string[] strategyTargetName = new string[3] { "对1", "对2", "对3"};
        public string[] strategyName = new string[6] { "立即","不使用","满np不用","绑定宝具", "绑定红爆", "绑定蓝卡"};

        public Form1()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();

            setComponent();
            //初始化一些Enabled
            for (int i = 0; i < 4; i++)
            {
                checkBuffButton[i].Enabled = false;
            }
            buttonEnd.Enabled = false;
            buttonSingle.Enabled = false;
            buttonMulti.Enabled = false;
            buttonR.Enabled = false;

            memory = new Memory();
            comboBoxInit();
            comboBoxRefresh();

            battlefield = new Battlefield(this);
        }
        private void setComponent()//初始化控件数组
        {
            servantBox = new GroupBox[3];
            for (int i = 0; i < 3; i++)
            {
                servantBox[i] = new GroupBox();
                servantBox[i].Name = "servantBox" + i.ToString();
                servantBox[i].Location = new System.Drawing.Point(30 + 330 * i, 20);
                servantBox[i].Size = new System.Drawing.Size(300, 450);
                servantBox[i].Text = "从者" + (i + 1).ToString();
                this.Controls.Add(servantBox[i]);
            }

            someBox = new GroupBox[2];

            someBox[0] = new GroupBox();
            someBox[0].Name = "someBox 0";
            someBox[0].Location = new System.Drawing.Point(30, 470);
            someBox[0].Size = new System.Drawing.Size(500, 300);
            someBox[0].Text = "上一回合结果显示";
            this.Controls.Add(someBox[0]);

            someBox[1] = new GroupBox();
            someBox[1].Name = "someBox 1";
            someBox[1].Location = new System.Drawing.Point(550, 470);
            someBox[1].Size = new System.Drawing.Size(480, 150);
            someBox[1].Text = "环境";
            this.Controls.Add(someBox[1]);

            //从者数据部分
            servantComboList = new ComboBox[3];//从者下拉框
            for (int i = 0; i < 3; i++)
            {
                servantComboList[i] = new ComboBox();
                servantComboList[i].Name = "servantComboList" + i.ToString();
                servantComboList[i].Location = new System.Drawing.Point(20, 50);
                servantComboList[i].Size = new System.Drawing.Size(270, 20);
                servantComboList[i].Font = new Font("Microsoft Sans Serif", 12);
                servantComboList[i].DropDownStyle = ComboBoxStyle.DropDownList;
                servantComboList[i].DropDownHeight = 250;
                servantComboList[i].SelectedIndexChanged += new System.EventHandler(this.servantComboList_SelectedIndexChanged);
                servantBox[i].Controls.Add(servantComboList[i]);
            }

            checkBuffButton = new Button[4];//查看buff按钮
            for (int i = 0; i < 3; i++)
            {
                checkBuffButton[i] = new Button();
                checkBuffButton[i].Name = "checkBuffButton" + i.ToString();
                checkBuffButton[i].Location = new System.Drawing.Point(20, 15);
                checkBuffButton[i].Size = new System.Drawing.Size(70, 30);
                checkBuffButton[i].Text = "查看buff";
                checkBuffButton[i].Click += new System.EventHandler(this.checkBuffButton_Click);
                servantBox[i].Controls.Add(checkBuffButton[i]);
            }
            checkBuffButton[3] = new Button();
            checkBuffButton[3].Name = "checkBuffButton" + 3.ToString();
            checkBuffButton[3].Location = new System.Drawing.Point(280, 90);
            checkBuffButton[3].Size = new System.Drawing.Size(120, 30);
            checkBuffButton[3].Text = "查看敌方debuff";
            checkBuffButton[3].Click += new System.EventHandler(this.checkBuffButton_Click);
            someBox[1].Controls.Add(checkBuffButton[3]);

            CEComboList = new ComboBox[3];//礼装下拉框
            for (int i = 0; i < 3; i++)
            {
                CEComboList[i] = new ComboBox();
                CEComboList[i].Name = "CEComboList" + i.ToString();
                CEComboList[i].Location = new System.Drawing.Point(20, 210);
                CEComboList[i].Size = new System.Drawing.Size(210, 20);
                CEComboList[i].Font = new Font("Microsoft Sans Serif", 12);
                CEComboList[i].DropDownStyle = ComboBoxStyle.DropDownList;
                CEComboList[i].DropDownHeight = 250;
                servantBox[i].Controls.Add(CEComboList[i]);

            }

            CEMLCheck = new CheckBox[3];//礼装是否满破
            for (int i = 0; i < 3; i++)
            {
                CEMLCheck[i] = new CheckBox();
                CEMLCheck[i].Name = "CEComboList" + i.ToString();
                CEMLCheck[i].Location = new System.Drawing.Point(240, 215);
                CEMLCheck[i].Size = new System.Drawing.Size(50, 20);
                CEMLCheck[i].Text = "满破";
                servantBox[i].Controls.Add(CEMLCheck[i]);
            }

            ATKText = new TextBox[3];
            for (int i = 0; i < 3; i++)
            {
                ATKText[i] = new TextBox();
                ATKText[i].Name = "ATKText" + i.ToString();
                ATKText[i].Location = new System.Drawing.Point(60, 90);
                ATKText[i].Size = new System.Drawing.Size(80, 20);
                servantBox[i].Controls.Add(ATKText[i]);
            }

            HPText = new TextBox[3];
            for (int i = 0; i < 3; i++)
            {
                HPText[i] = new TextBox();
                HPText[i].Name = "HPText" + i.ToString();
                HPText[i].Location = new System.Drawing.Point(200, 90);
                HPText[i].Size = new System.Drawing.Size(80, 20);
                servantBox[i].Controls.Add(HPText[i]);
            }

            NPText = new TextBox[3];
            for (int i = 0; i < 3; i++)
            {
                NPText[i] = new TextBox();
                NPText[i].Name = "NPText" + i.ToString();
                NPText[i].Location = new System.Drawing.Point(60, 130);
                NPText[i].Size = new System.Drawing.Size(80, 20);
                servantBox[i].Controls.Add(NPText[i]);
            }

            sumNPText = new TextBox[3];
            for (int i = 0; i < 3; i++)
            {
                sumNPText[i] = new TextBox();
                sumNPText[i].Name = "sumNPText" + i.ToString();
                sumNPText[i].Location = new System.Drawing.Point(200, 130);
                sumNPText[i].Size = new System.Drawing.Size(80, 20);
                servantBox[i].Controls.Add(sumNPText[i]);
            }

            ADMGText = new TextBox[3];
            for (int i = 0; i < 3; i++)
            {
                ADMGText[i] = new TextBox();
                ADMGText[i].Name = "ADMGText" + i.ToString();
                ADMGText[i].Location = new System.Drawing.Point(60, 170);
                ADMGText[i].Size = new System.Drawing.Size(80, 20);
                servantBox[i].Controls.Add(ADMGText[i]);
            }

            NPDMGText = new TextBox[3];
            for (int i = 0; i < 3; i++)
            {
                NPDMGText[i] = new TextBox();
                NPDMGText[i].Name = "NPDMGText" + i.ToString();
                NPDMGText[i].Location = new System.Drawing.Point(200, 170);
                NPDMGText[i].Size = new System.Drawing.Size(80, 20);
                servantBox[i].Controls.Add(NPDMGText[i]);
            }

            servantLabel = new Label[18];
            for (int i = 0; i < 18; i++)
            {
                servantLabel[i] = new Label();
                servantLabel[i].Name = "servantLabel" + i.ToString();
                servantLabel[i].Location = new System.Drawing.Point(10 + 140 * (i % 2), 90 + 40 * ((i % 6) / 2));
                servantLabel[i].Size = new System.Drawing.Size(55, 20);
                if (i % 6 == 0) servantLabel[i].Text = "ATK";
                if (i % 6 == 1) servantLabel[i].Text = "HP";
                if (i % 6 == 2) servantLabel[i].Text = "现有np";
                if (i % 6 == 3) servantLabel[i].Text = "10T np";
                if (i % 6 == 4) servantLabel[i].Text = "10T A伤";
                if (i % 6 == 5) servantLabel[i].Text = "10T 宝伤";
                servantBox[i / 6].Controls.Add(servantLabel[i]);
            }

            //从者AI部分
            colorButton = new Button[16];//颜色卡的按钮
            for (int i = 0; i < 16; i++)
            {
                colorButton[i] = new Button();
                colorButton[i].Name = "colorButton" + i.ToString();
                colorButton[i].Size = new System.Drawing.Size(50, 30);
                if (i % 4 == 0) colorButton[i].BackColor = Color.Red;
                if (i % 4 == 1) colorButton[i].BackColor = Color.Blue;
                if (i % 4 == 2) colorButton[i].BackColor = Color.Green;
                if (i % 4 == 3) colorButton[i].BackColor = Color.White;
                if (i / 4 != 3)
                {
                    colorButton[i].Location = new System.Drawing.Point(50 + 60 * (i % 4), 240);
                    servantBox[i / 4].Controls.Add(colorButton[i]);
                }
                else
                {
                    colorButton[i].Location = new System.Drawing.Point(230 + 60 * (i % 4), 10);
                    someBox[1].Controls.Add(colorButton[i]);
                }
            }

            priorityText = new TextBox[3*3+4];//行动卡优先级
            for (int i = 0; i < 3*3+4; i++)
            {
                priorityText[i] = new TextBox();
                priorityText[i].Name = "proirityText" + i.ToString();
                priorityText[i].Size = new System.Drawing.Size(50, 30);
                if (i<9)
                {
                    priorityText[i].Location = new System.Drawing.Point(50 + 60 * (i % 3), 280);
                    servantBox[i / 3].Controls.Add(priorityText[i]);
                    priorityText[i].Text = ((3-i/3)*10+((i%3)+1)%3).ToString();
                }
                else
                {
                    priorityText[i].Location = new System.Drawing.Point(230 + 60 * (i - 9), 50);
                    someBox[1].Controls.Add(priorityText[i]);
                    if (i == 9) priorityText[i].Text = "8";
                    if (i == 10) priorityText[i].Text = "50";
                    if (i== 11) priorityText[i].Text = "5";
                    if (i== 12) priorityText[i].Text = "20";
                }
            }

            skillButton = new Button[9];//显示CD的技能按钮
            for (int i = 0; i < 9; i++)
            {
                skillButton[i] = new Button();
                skillButton[i].Name = "skillButton" + i.ToString();
                skillButton[i].Location = new System.Drawing.Point(50 + 60 * (i % 3), 320);
                skillButton[i].Size = new System.Drawing.Size(50, 50);
                skillButton[i].Text = "技能" + ((i % 3) + 1).ToString();
                skillButton[i].Click += new System.EventHandler(this.skillButton_Click);
                servantBox[i / 3].Controls.Add(skillButton[i]);
            }

            strategyComboList = new ComboBox[18];//技能策略下拉框
            for (int i = 0; i < 18; i++)
            {
                strategyComboList[i] = new ComboBox();
                strategyComboList[i].Name = "strategyText" + i.ToString();
                strategyComboList[i].Location = new System.Drawing.Point(50 + 60 * (i % 3), 380 + (i % 6) / 3 * 40);
                strategyComboList[i].Size = new System.Drawing.Size(55, 30);
                strategyComboList[i].DropDownStyle = ComboBoxStyle.DropDownList;
                strategyComboList[i].DropDownHeight = 200;
                servantBox[i / 6].Controls.Add(strategyComboList[i]);

            }

            NPButton = new Button[3];//宝具按钮
            for (int i = 0; i < 3; i++)
            {
                NPButton[i] = new Button();
                NPButton[i].Name = "NPButton" + (i + 9).ToString();
                NPButton[i].Location = new System.Drawing.Point(50 + 60 * 3, 320);
                NPButton[i].Size = new System.Drawing.Size(50, 25);
                NPButton[i].Text = "宝具";
                NPButton[i].Click += new System.EventHandler(this.skillButton_Click);
                servantBox[i].Controls.Add(NPButton[i]);
            }

            NPNumeric = new NumericUpDown[3];//几宝选择
            for (int i = 0; i < 3; i++)
            {
                NPNumeric[i] = new NumericUpDown();
                NPNumeric[i].Name = "NPNumeric" + i.ToString();
                NPNumeric[i].Location = new System.Drawing.Point(50 + 60 * 3, 350);
                NPNumeric[i].Size = new System.Drawing.Size(50, 25);
                NPNumeric[i].Maximum = 5;
                NPNumeric[i].Minimum = 1;
                NPNumeric[i].Value = 1;
                servantBox[i].Controls.Add(NPNumeric[i]);
            }

            NPpriorityText = new TextBox[3];//宝具优先级
            for (int i = 0; i < 3; i++)
            {
                NPpriorityText[i] = new TextBox();
                NPpriorityText[i].Name = "NPpriorityText" + i.ToString();
                NPpriorityText[i].Location = new System.Drawing.Point(50 + 60 * 3, 380);
                NPpriorityText[i].Size = new System.Drawing.Size(50, 30);
                NPpriorityText[i].Text = "60";
                servantBox[i].Controls.Add(NPpriorityText[i]);
            }

            servantAILabel = new Label[12 + 2];
            for (int i = 0; i < 12; i++)
            {
                servantAILabel[i] = new Label();
                servantAILabel[i].Name = "servantAILabel" + i.ToString();
                servantAILabel[i].Location = new System.Drawing.Point(10, 250 + 40 * (i % 4));
                servantAILabel[i].Size = new System.Drawing.Size(55, 20);
                if (i % 4 == 0) servantAILabel[i].Text = "行动卡";
                if (i % 4 == 1) servantAILabel[i].Text = "优先级";
                if (i % 4 == 2) servantAILabel[i].Text = "技能";
                if (i % 4 == 3) servantAILabel[i].Text = "优先级";
                servantBox[i / 4].Controls.Add(servantAILabel[i]);
            }
            servantAILabel[12] = new Label();
            servantAILabel[12].Name = "servantAILabel11";
            servantAILabel[12].Location = new System.Drawing.Point(185, 10);
            servantAILabel[12].Size = new System.Drawing.Size(55, 20);
            servantAILabel[12].Text = "chain";
            someBox[1].Controls.Add(servantAILabel[12]);
            servantAILabel[13] = new Label();
            servantAILabel[13].Name = "servantAILabel12";
            servantAILabel[13].Location = new System.Drawing.Point(185, 50);
            servantAILabel[13].Size = new System.Drawing.Size(55, 20);
            servantAILabel[13].Text = "至少10";
            someBox[1].Controls.Add(servantAILabel[13]);

            specialCheck = new CheckBox[3];//是否计算特攻
            for (int i = 0; i < 3; i++)
            {
                specialCheck[i] = new CheckBox();
                specialCheck[i].Name = "specialCheck" + i.ToString();
                specialCheck[i].Location = new System.Drawing.Point(210, 20);
                specialCheck[i].Size = new System.Drawing.Size(80, 20);
                specialCheck[i].Text = "开启特攻";
                servantBox[i].Controls.Add(specialCheck[i]);
            }

            //结果部分
            resultButton = new Button[3 + 5 + 4];
            for (int i = 0; i < 5; i++)//候选卡
            {
                resultButton[i] = new Button();
                resultButton[i].Name = "resultButton" + i.ToString();
                resultButton[i].Location = new System.Drawing.Point(40 + 90 * i, 20 + 50);
                resultButton[i].Size = new System.Drawing.Size(80, 40);
                resultButton[i].ForeColor = Color.White;
                someBox[0].Controls.Add(resultButton[i]);
            }
            for (int i = 5; i < 5 + 3; i++)//宝具卡
            {
                resultButton[i] = new Button();
                resultButton[i].Name = "resultButton" + i.ToString();
                resultButton[i].Location = new System.Drawing.Point(130 + 90 * (i - 5), 20);
                resultButton[i].Size = new System.Drawing.Size(80, 40);
                resultButton[i].ForeColor = Color.White;
                someBox[0].Controls.Add(resultButton[i]);
            }
            for (int i = 5 + 3; i < 5 + 3 + 4; i++)//选择的卡
            {
                resultButton[i] = new Button();
                resultButton[i].Name = "resultButton" + i.ToString();
                resultButton[i].Location = new System.Drawing.Point(95 + 90 * (i - 5 - 3), 20 + 100);
                resultButton[i].Size = new System.Drawing.Size(80, 40);
                resultButton[i].ForeColor = Color.White;
                someBox[0].Controls.Add(resultButton[i]);
            }
            resultButton[5 + 3 + 4 - 1].ForeColor = Color.Black;

            resultText = new TextBox[4];//各选择卡结果显示
            for (int i = 0; i < 4; i++)
            {
                resultText[i] = new TextBox();
                resultText[i].Name = "resultText" + i.ToString();
                resultText[i].Location = new System.Drawing.Point(95 + 90 * i, 170);
                resultText[i].Size = new System.Drawing.Size(80, 100);
                resultText[i].Multiline = true;
                someBox[0].Controls.Add(resultText[i]);
            }
            resultLabel = new Label[4];
            for (int i = 0; i < 4; i++)
            {
                resultLabel[i] = new Label();
                resultLabel[i].Name = "resultLabel" + i.ToString();
                resultLabel[i].Location = new System.Drawing.Point(10, 30 + 50 * i);
                resultLabel[i].Size = new System.Drawing.Size(80, 30);
                someBox[0].Controls.Add(resultLabel[i]);
            }
            resultLabel[0].Text = "宝具";
            resultLabel[1].Text = "候选";
            resultLabel[2].Text = "选择";
            resultLabel[3].Text = "结果";

            //环境部分
            enviNumeric = new NumericUpDown[2];//控制敌我从者数量数字框
            for (int i = 0; i < 2; i++)
            {
                enviNumeric[i] = new NumericUpDown();
                enviNumeric[i].Name = "enviNumeric " + i.ToString();
                enviNumeric[i].Location = new System.Drawing.Point(100, 70 + 30 * i);
                enviNumeric[i].Size = new System.Drawing.Size(55, 20);
                enviNumeric[i].Maximum = 3;
                enviNumeric[i].Minimum = 1;
                enviNumeric[i].Value = 3;
                someBox[1].Controls.Add(enviNumeric[i]);
            }
            enviNumeric[0].Click += new System.EventHandler(this.servantnumericUpDown_ValueChanged);

            enviLabel = new Label[2];
            for (int i = 0; i < 2; i++)
            {
                enviLabel[i] = new Label();
                enviLabel[i].Name = "enviLabel " + i.ToString();
                enviLabel[i].Location = new System.Drawing.Point(20, 70 + 30 * i);
                enviLabel[i].Size = new System.Drawing.Size(75, 20);
                someBox[1].Controls.Add(enviLabel[i]);
            }
            enviLabel[0].Text = "我方从者数";
            enviLabel[1].Text = "敌方从者数";

            enviButton = new Button[2];//环境按钮
            for (int i = 0; i < 2; i++)
            {
                enviButton[i] = new Button();//修改环境按钮
                enviButton[i].Name = "enviButton " + i.ToString();
                enviButton[i].Location = new System.Drawing.Point(20 + 80 * i, 20);
                enviButton[i].Size = new System.Drawing.Size(75, 30);
                someBox[1].Controls.Add(enviButton[i]);
            }
            enviButton[0].Text = "修改数据";
            enviButton[0].Click += new System.EventHandler(this.modifyButton_Click);
            enviButton[1].Text = "更新数据";
            enviButton[1].Click += new System.EventHandler(this.renewButton_Click);

            enemyServantClassCombo = new ComboBox();//修改敌方阶级
            enemyServantClassCombo.Name = "enemyServantClassCombo 0";
            enemyServantClassCombo.Location = new System.Drawing.Point(170, 100);
            enemyServantClassCombo.Size = new System.Drawing.Size(100, 30);
            enemyServantClassCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            enemyServantClassCombo.DropDownHeight = 150;
            someBox[1].Controls.Add(enemyServantClassCombo);

        }

        private void comboBoxInit()//初始化只需一次的combolist
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < strategyTargetName.Length; j++)
                {
                    for (int m = 0; m < 3; m++)
                    {
                        strategyComboList[i * 6 + m].Items.Add(strategyTargetName[j]);
                        strategyComboList[i * 6 + m].Text = strategyTargetName[0];
                    }
                }
                for (int j = 0; j < strategyName.Length; j++)
                {
                    for (int m = 0; m < 3; m++)
                    {
                        strategyComboList[i * 6 + 3 + m].Items.Add(strategyName[j]);
                        strategyComboList[i * 6 + 3 + m].Text = strategyName[0];
                    }
                }
            }

            for (int i = 0; i < className.Length; i++)
            {
                enemyServantClassCombo.Items.Add(className[i]);
                enemyServantClassCombo.Text = "berserker";
            }
        }
        private void comboBoxRefresh()//更新从者和礼装的combolist
        {
            for (int i = 0; i < 3; i++)
            {
                memory.refreshComboBox(servantComboList[i]);
                memory.showComboBox(servantComboList[i], 2);

                memory.refreshCEComboBox(CEComboList[i]);
                CEComboList[i].Text = "0 无礼装";

            }
        }

        public int getNoFromName(string[] list, string name)//得到在某数组中的位置
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (name == list[i])
                {
                    return i;
                }
            }
            return 0;
        }
        public int getTNoFromName(string TName)//把最后2个的数字部分取出
        {
            int temp = TName[TName.Length - 1] - '0';
            if (TName[TName.Length - 2] >= '0' && TName[TName.Length - 2] <= '9')
            {
                temp += 10 * (TName[TName.Length - 2] - '0');
            }
            return temp;
        }

        public void skillUsed(int servantNo,int skillNo)
        {
            skillButton[servantNo * 3 + skillNo].BackColor = Color.Yellow;
        }
        public void skillClear()
        {
            for (int i = 0; i < 9; i++)
            {
                skillButton[i].BackColor = Color.White;
            }
        }

        private void displayServant()//将模拟的最新信息更新进从者显示界面
        {
            for (int i = 0; i < battlefield.servantNum; i++)
            {
                NPText[i].Text = Math.Round(battlefield.allyServant[i].nownp).ToString();
                sumNPText[i].Text = Math.Round(battlefield.allyServant[i].sumnp * 10.0 / battlefield.turnNum).ToString();
                ADMGText[i].Text = Math.Round(battlefield.allyServant[i].sumAdmg * 10.0 / battlefield.turnNum).ToString();
                NPDMGText[i].Text = Math.Round(battlefield.allyServant[i].sumNPdmg * 10.0 / battlefield.turnNum).ToString();

                for (int j = 0; j < 3; j++)
                {
                    skillButton[3 * i + j].Text = "剩余CD " + battlefield.allyServant[i].skills[j].leftCD.ToString();
                }
            }
        }

        private void readFromTable()//从桌面读取从者信息
        {
            int servantNum = (int)enviNumeric[0].Value;
            int targetNum = (int)enviNumeric[1].Value;
            battlefield.setNN(servantNum, targetNum);

            for (int i = 0; i < servantNum; i++)
            {
                battlefield.allyServant[i].copyS(memory.getServantWithComboText(servantComboList[i].Text));

                battlefield.allyServant[i].ATK = int.Parse(ATKText[i].Text);
                battlefield.allyServant[i].HP = int.Parse(HPText[i].Text);
                battlefield.allyServant[i].specialATK = specialCheck[i].Checked;
                battlefield.allyServant[i].NPdmg = battlefield.calculateNPdmg(i, (int)NPNumeric[i].Value);

                battlefield.allyServant[i].equippedCE.copy(memory.getCEWithComboText(CEComboList[i].Text));
            }

            for (int i = 0; i < targetNum; i++)
            {
                battlefield.enemyServant[i].servantClass = getNoFromName(className, enemyServantClassCombo.Text);
            }
            //初始化AI数据
            for (int i = 0; i < 18; i++)
            {
                if (i % 6 < 3)
                {
                    battlefield.strategy[i] = getNoFromName(strategyTargetName, strategyComboList[i].Text);
                }
                else
                {
                    battlefield.strategy[i] = getNoFromName(strategyName, strategyComboList[i].Text);
                }
            }
            for (int i = 0; i < 3*3+4; i++)
            {
                battlefield.priority[i] = int.Parse(priorityText[i].Text);
            }
            for (int i = 0; i < 3; i++)
            {
                battlefield.NPpriority[i] = int.Parse(NPpriorityText[i].Text);
            }

        }

        private void displayResult(ResultData result)//将得到结果展示在结果界面
        {
            for (int i = 0; i < 3 + 5 + 4; i++)
            {
                resultButton[i].BackColor = Color.Black;
                resultButton[i].Text = "";
            }

            for (int i = 0; i < 3+5; i++)
            {
                if (i < result.candidateHits.Count)
                {
                    if (i < 5 + 3 && i >= 5)
                    {
                        if (result.candidateHits[i].hitType == 1) resultButton[5+result.candidateHits[i].attackerNo].BackColor = Color.Red;
                        if (result.candidateHits[i].hitType == 2) resultButton[5+result.candidateHits[i].attackerNo].BackColor = Color.Blue;
                        if (result.candidateHits[i].hitType == 3) resultButton[5+result.candidateHits[i].attackerNo].BackColor = Color.Green;
                        resultButton[5+result.candidateHits[i].attackerNo].Text = battlefield.allyServant[result.candidateHits[i].attackerNo].name;
                    }
                    else
                    {
                        if (result.candidateHits[i].hitType == 0) resultButton[i].BackColor = Color.White;
                        if (result.candidateHits[i].hitType == 1) resultButton[i].BackColor = Color.Red;
                        if (result.candidateHits[i].hitType == 2) resultButton[i].BackColor = Color.Blue;
                        if (result.candidateHits[i].hitType == 3) resultButton[i].BackColor = Color.Green;
                        resultButton[i].Text = (result.candidateHits[i].attackerNo+1).ToString()+"号位\r\n" + battlefield.allyServant[result.candidateHits[i].attackerNo].name;
                    }
                }
            }

            resultButton[3+5+3].BackColor = Color.Black;
            resultText[3].Text = "";
            for (int i = 0; i < result.pickedHits.Count; i++)
            {
                if (result.pickedHits[i].hitType == 0) resultButton[3+5+i].BackColor = Color.White;
                if (result.pickedHits[i].hitType == 1) resultButton[3+5+i].BackColor = Color.Red;
                if (result.pickedHits[i].hitType == 2) resultButton[3+5+i].BackColor = Color.Blue;
                if (result.pickedHits[i].hitType == 3) resultButton[3+5+i].BackColor = Color.Green;
                resultButton[3 + 5 + i].Text = (result.pickedHits[i].attackerNo + 1).ToString()+"号位\r\n"+battlefield.allyServant[result.pickedHits[i].attackerNo].name;
                resultText[i].Text = "获得np: " + Math.Round(result.pickedHits[i].getnp).ToString() + "\r\n\r";
                resultText[i].Text += "获得星星: " + result.pickedHits[i].getStar.ToString() + "\r\n\r";

                if (result.pickedHits[i].MakeAdmg > 0)
                {
                    resultText[i].Text += "是否暴击: " + ((result.pickedHits[i].isCrit) ? "是" : "否") + "\r\n\r\n";
                    resultText[i].Text += "平A伤害:\r\n" + Math.Round(result.pickedHits[i].MakeAdmg).ToString() + "\r\n";
                }
                else
                {
                    resultText[i].Text += "\r\n";
                    resultText[i].Text += "宝具伤害: " + Math.Round(result.pickedHits[i].MakeNPdmg).ToString() + "\r\n";
                }
            }
        }

        private void servantComboList_SelectedIndexChanged(object sender, System.EventArgs e)//下拉选择一个从者，将数据展示在从者界面
        {
            Servant servant = memory.getServantWithComboText(((ComboBox)sender).SelectedItem.ToString());
            int objectNo = getTNoFromName(((ComboBox)sender).Name);

            ATKText[objectNo].Text = servant.ATK.ToString();
            HPText[objectNo].Text = servant.HP.ToString();
            
        }
        
        private void servantnumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            servantnumericUpDownChange();
        }

        private void servantnumericUpDownChange()//从者数量改变时对box的Enabled进行更改
        {
            for (int i = 1; i < 3; i++)
            {
                if (i < ((int)enviNumeric[0].Value))
                {
                    servantBox[i].Enabled = true;
                }
                else
                {
                    servantBox[i].Enabled = false;
                }
            }
        }

        private void checkBuffButton_Click(object sender, EventArgs e)//点击查看buff
        {
            int senderNo=getTNoFromName(((Button)sender).Name);
            MessageBox.Show(battlefield.showBuff((int)(senderNo)));
        }

        private void skillButton_Click(object sender, EventArgs e)//点击查看技能
        {
            int objectNo = getTNoFromName(((Button)sender).Name);
            Servant servant = new Servant();
            if (objectNo < 9)
            {
                servant = memory.getServantWithComboText(servantComboList[objectNo / 3].SelectedItem.ToString());
            }
            else
            {
                servant = memory.getServantWithComboText(servantComboList[objectNo - 9].SelectedItem.ToString());
            }

            string result = "";
            int skillNo = 0;
            if (objectNo < 9)
            {
                skillNo = objectNo%3;
            }
            else
            {
                skillNo = 4;
            }

            for (int i = 0; i < servant.skills[skillNo].getEffectNum(); i++)
            {
                result += "对象类型  " + targetTypeName[servant.skills[skillNo].getEffect(i).targetType] + "\r\n";
                result += "技能类型  " + effectName[servant.skills[skillNo].getEffect(i).type] + "\r\n";
                result += "数值  " + servant.skills[skillNo].getEffect(i).value.ToString() + "\r\n";
                result += "持续时间  " + servant.skills[skillNo].getEffect(i).turn.ToString() + "\r\n";
                result += "概率  " + servant.skills[skillNo].getEffect(i).probability.ToString()+"\r\n\r\n";
            }
            
            MessageBox.Show(result);
        }

        private void buttonMulti_Click(object sender, EventArgs e)//多次模拟
        {
            int testNum = int.Parse(testTimes.Text);
            ResultData result = new ResultData();
            for (int t = 0; t < testNum; t++)//test "testNum" turns
            {
                result=battlefield.oneTurn();
            }
            displayResult(result);
            displayServant();

            double overall = 0;
            for (int i = 0; i < battlefield.servantNum; i++)
            {
                overall += battlefield.allyServant[i].sumAdmg  / battlefield.turnNum/1000.0;
                overall += battlefield.allyServant[i].sumNPdmg  / battlefield.turnNum/1000.0;
            }

            MessageBox.Show("模拟完毕\r\n全队平均10T伤害:"+ Math.Round(overall).ToString()+"万");
        }

        private void buttonSingle_Click(object sender, EventArgs e)//单次模拟
        {
            ResultData result = battlefield.oneTurn();
            displayResult(result);

            displayServant();           
            
        }        

        private void buttonStart_Click(object sender, EventArgs e)//开始模拟
        {
            buttonStart.Enabled = false;
            buttonEnd.Enabled = true;
            buttonSingle.Enabled = true;
            buttonMulti.Enabled = true;
            buttonR.Enabled = true;

            for (int i = 0; i < 2; i++)
            {
                enviButton[i].Enabled = false;
                enviNumeric[i].Enabled = false;
            }
            for (int i = 0; i < 3; i++)
            {
                checkBuffButton[i].Enabled = true;
                servantComboList[i].Enabled = false;
                CEComboList[i].Enabled = false;
                CEMLCheck[i].Enabled = false;
                specialCheck[i].Enabled = false;
                ATKText[i].Enabled = false;
                HPText[i].Enabled = false;
                NPNumeric[i].Enabled = false;
                NPpriorityText[i].Enabled = false;
            }
            checkBuffButton[3].Enabled = true;
            for (int i = 0; i < 18; i++)
            {
                strategyComboList[i].Enabled = false;
            }
            for (int i = 0; i < 3*3+4; i++)
            {
                priorityText[i].Enabled = false;
            }
            enemyServantClassCombo.Enabled = false;

            battlefield.refresh();
            readFromTable();
            battlefield.initPassiveSkill();

            bool[] isMaxLimit = new bool[3];
            for (int i = 0; i < 3; i++)
            {
                isMaxLimit[i] = CEMLCheck[i].Checked;
            }
            battlefield.initCE(isMaxLimit);

        }

        private void buttonEnd_Click(object sender, EventArgs e)//结束模拟
        {
            buttonStart.Enabled = true;
            buttonEnd.Enabled = false;
            buttonSingle.Enabled = false;
            buttonMulti.Enabled = false;
            buttonR.Enabled = false;




            for (int i = 0; i < 2; i++)
            {
                enviButton[i].Enabled = true;
                enviNumeric[i].Enabled = true;
            }
            for (int i = 0; i < 3; i++)
            {
                checkBuffButton[i].Enabled = false;
                servantComboList[i].Enabled = true;
                CEComboList[i].Enabled = true;
                CEMLCheck[i].Enabled = true;
                specialCheck[i].Enabled = true;
                ATKText[i].Enabled = true;
                HPText[i].Enabled = true;
                NPNumeric[i].Enabled = true;
                NPpriorityText[i].Enabled = true;
            }
            checkBuffButton[3].Enabled = false;
            for (int i = 0; i < 18; i++)
            {
                strategyComboList[i].Enabled = true;
            }
            for (int i = 0; i < 3*3+4; i++)
            {
                priorityText[i].Enabled = true;
            }
            enemyServantClassCombo.Enabled = true;


            servantnumericUpDownChange();
        }

        private void buttonR_Click(object sender, EventArgs e)//模拟过程中数据重置
        {
            battlefield.refresh();
            displayServant();
        }

        private void modifyButton_Click(object sender, EventArgs e)//打开修改文件窗口
        {
            formRW = new FormSelectRW(this);
            formRW.Visible = true;
        }

        private void renewButton_Click(object sender, EventArgs e)//根据修改了的文件更新界面
        {
            comboBoxRefresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<double> allvalues=new List<double>();
            List<string> allnames=new List<string>();


            for (int j = 0; j < CEComboList[0].Items.Count; j++)
            {
                battlefield.refresh();
                readFromTable();
                battlefield.allyServant[0].equippedCE.copy(memory.getCEWithComboText(CEComboList[0].GetItemText(CEComboList[0].Items[j])));
                battlefield.initPassiveSkill();
                bool[] isMaxLimit = new bool[3];
                for (int i = 0; i < 3; i++)
                {
                    isMaxLimit[i] = CEMLCheck[i].Checked;
                }
                battlefield.initCE(isMaxLimit);

                int testNum = int.Parse(testTimes.Text);
                ResultData result = new ResultData();
                for (int t = 0; t < testNum; t++)//test "testNum" turns
                {
                    result = battlefield.oneTurn();
                }

                double overall = 0;
                for (int i = 0; i < battlefield.servantNum; i++)
                {
                    overall += battlefield.allyServant[i].sumAdmg / battlefield.turnNum / 1000.0;
                    overall += battlefield.allyServant[i].sumNPdmg / battlefield.turnNum / 1000.0;
                }

                allvalues.Add(overall);
                allnames.Add(CEComboList[0].GetItemText(CEComboList[0].Items[j]));
            }

            for (int j = 0; j < allvalues.Count; j++)
            {
                for (int i = allvalues.Count-1; i >j; i--)
                {
                    if (allvalues[i - 1] < allvalues[i])
                    {
                        //swap i-1 i
                        double temp1 = allvalues[i - 1];
                        allvalues[i - 1] = allvalues[i];
                        allvalues[i] = temp1;
                        string temp2 = allnames[i - 1];
                        allnames[i - 1]= allnames[i];
                        allnames[i] = temp2;
                    }

                }
            }
            string temp3 = "";
            for (int j = 0; j < allvalues.Count && j < 10; j++)
            {
                temp3 += allnames[j]+" "+Math.Round(allvalues[j]).ToString()+"\r\n";
            }
            MessageBox.Show(temp3);
        }
    }

    public class Servant
    {
        public int ID;
        public string name;
        public int rarity;
        public int servantClass;
        public int cost;

        public int ATK;
        public int HP;
        public int attribute;
        public double npCharge;
        public double npChargeDEF;
        public double starGe;
        public int starAb;

        public int redCards;
        public int redHits;
        public int blueCards;
        public int blueHits;
        public int greenCards;
        public int greenHits;
        public int EXHits;

        public int NPtype;
        public double NPdmg;
        public int NPHits;
        public int NPtarget;

        public Skill[] skills;//0-2为技能，3为被动，4NP部分效果

        public Servant()
        {
            skills = new Skill[5];
            for (int i = 0; i < 5; i++)
            {
                skills[i] = new Skill();
            }
        }

        public Servant(string memoryString)//从一行数据得到新的从者
        {
            string[] sArray = memoryString.Split(' ');
            int point = 0;

            ID = int.Parse(sArray[point++]);
            name = sArray[point++];
            rarity = int.Parse(sArray[point++]);
            servantClass = int.Parse(sArray[point++]);
            cost = int.Parse(sArray[point++]);

            ATK = int.Parse(sArray[point++]);
            HP = int.Parse(sArray[point++]);
            attribute = int.Parse(sArray[point++]);
            npCharge = double.Parse(sArray[point++]);
            npChargeDEF = double.Parse(sArray[point++]);
            starGe = double.Parse(sArray[point++]);
            starAb = int.Parse(sArray[point++]);

            redCards = int.Parse(sArray[point++]);
            redHits = int.Parse(sArray[point++]);
            blueCards = int.Parse(sArray[point++]);
            blueHits = int.Parse(sArray[point++]);
            greenCards = int.Parse(sArray[point++]);
            greenHits = int.Parse(sArray[point++]);
            EXHits = int.Parse(sArray[point++]);

            NPtype = int.Parse(sArray[point++]);
            NPdmg = double.Parse(sArray[point++]);
            NPHits = int.Parse(sArray[point++]);
            NPtarget = int.Parse(sArray[point++]);

            skills = new Skill[5];
            for (int i = 0; i < 5; i++)
            {
                skills[i] = new Skill();
            }

            skills[4].clearEffect();
            int NPENum = int.Parse(sArray[point++]);
            for (int i = 0; i < NPENum; i++)
            {
                int iTargetType = int.Parse(sArray[point++]);
                int iType = int.Parse(sArray[point++]);
                double iValue = double.Parse(sArray[point++]);
                int iTurn = int.Parse(sArray[point++]);
                double iProbability = double.Parse(sArray[point++]);
                bool iafterNP = (sArray[point++] == "1") ? true : false;
                skills[4].addEffect(iTargetType, iType, iValue, iTurn, iProbability, iafterNP);
            }

            for (int j = 0; j < 4; j++)
            {
                skills[j].CD = int.Parse(sArray[point++]);
                skills[j].clearEffect();
                int skillENum = int.Parse(sArray[point++]);
                for (int i = 0; i < skillENum; i++)
                {
                    int iTargetType = int.Parse(sArray[point++]);
                    int iType = int.Parse(sArray[point++]);
                    double iValue = double.Parse(sArray[point++]);
                    int iTurn = int.Parse(sArray[point++]);
                    double iProbability = double.Parse(sArray[point++]);
                    skills[j].addEffect(iTargetType, iType, iValue, iTurn, iProbability, false);
                }
            }

        }

        public string writeS()//编写一行从者数据
        {
            string tempstr = "";

            tempstr += ID.ToString() + " ";
            tempstr += name + " ";
            tempstr += rarity.ToString() + " ";
            tempstr += servantClass.ToString() + " ";
            tempstr += cost.ToString() + " ";

            tempstr += ATK.ToString() + " ";
            tempstr += HP.ToString() + " ";
            tempstr += attribute.ToString() + " ";
            tempstr += npCharge.ToString() + " ";
            tempstr += npChargeDEF.ToString() + " ";
            tempstr += starGe.ToString() + " ";
            tempstr += starAb.ToString() + " ";

            tempstr += redCards.ToString() + " ";
            tempstr += redHits.ToString() + " ";
            tempstr += blueCards.ToString() + " ";
            tempstr += blueHits.ToString() + " ";
            tempstr += greenCards.ToString() + " ";
            tempstr += greenHits.ToString() + " ";
            tempstr += EXHits.ToString() + " ";

            tempstr += NPtype.ToString() + " ";
            tempstr += NPdmg.ToString() + " ";
            tempstr += NPHits.ToString() + " ";
            tempstr += NPtarget.ToString() + " ";

            tempstr += skills[4].getEffectNum().ToString() + " ";
            for (int i = 0; i < skills[4].getEffectNum(); i++)
            {
                SingleEffect tempEff = skills[4].getEffect(i);
                tempstr += tempEff.targetType.ToString() + " ";
                tempstr += tempEff.type.ToString() + " ";
                tempstr += tempEff.value.ToString() + " ";
                tempstr += tempEff.turn.ToString() + " ";
                tempstr += tempEff.probability.ToString() + " ";
                tempstr += (tempEff.afterNP == true) ? "1 " : "0 ";
            }

            for (int j = 0; j < 4; j++)
            {
                tempstr += skills[j].CD.ToString() + " ";
                tempstr += skills[j].getEffectNum().ToString() + " ";
                for (int i = 0; i < skills[j].getEffectNum(); i++)
                {
                    SingleEffect tempEff = skills[j].getEffect(i);
                    tempstr += tempEff.targetType.ToString() + " ";
                    tempstr += tempEff.type.ToString() + " ";
                    tempstr += tempEff.value.ToString() + " ";
                    tempstr += tempEff.turn.ToString() + " ";
                    tempstr += tempEff.probability.ToString() + " ";
                }
            }
            return tempstr;
        }
    }
    public class CraftEssence
    {
        public int ID;
        public string name;
        public int rarity;
        public int cost;
        public int ATK;
        public int HP;
        public bool maxLimit;

        private List<SingleEffect> effects;

        public CraftEssence()
        {
            effects = new List<SingleEffect>();
        }

        public CraftEssence(string memoryString)//从一行数据得到新的礼装
        {
            string[] sArray = memoryString.Split(' ');
            int point = 0;

            ID = int.Parse(sArray[point++]);
            name = sArray[point++];
            rarity = int.Parse(sArray[point++]);
            cost = int.Parse(sArray[point++]);
            ATK = int.Parse(sArray[point++]);
            HP = int.Parse(sArray[point++]);

            effects = new List<SingleEffect>();
            int ENum = int.Parse(sArray[point++]);
            for (int i = 0; i < ENum; i++)
            {
                int iTargetType = int.Parse(sArray[point++]);
                int iType = int.Parse(sArray[point++]);
                double iValue = double.Parse(sArray[point++]);
                int iTurn = int.Parse(sArray[point++]);
                double iFullvalue = double.Parse(sArray[point++]);
                effects.Add(new SingleEffect(iTargetType, iType, iValue, iTurn, 1, false, iFullvalue));
            }

        }

        public string writeCE()//编写一行礼装数据
        {
            string tempstr = "";

            tempstr += ID.ToString() + " ";
            tempstr += name + " ";
            tempstr += rarity.ToString() + " ";
            tempstr += cost.ToString() + " ";
            tempstr += ATK.ToString() + " ";
            tempstr += HP.ToString() + " ";

            tempstr += effects.Count.ToString() + " ";
            for (int i = 0; i < effects.Count; i++)
            {
                SingleEffect tempEff = effects[i];
                tempstr += tempEff.targetType.ToString() + " ";
                tempstr += tempEff.type.ToString() + " ";
                tempstr += tempEff.value.ToString() + " ";
                tempstr += tempEff.turn.ToString() + " ";
                tempstr += tempEff.fullvalue.ToString() + " ";
            }

            return tempstr;
        }

        public void addEffect(int targetType, int type, double value, int turn, double probability, bool afterNP, double fullvalue)//初始化的时候用来添加效果
        {
            SingleEffect tempE = new SingleEffect(targetType, type, value, turn, probability, afterNP, fullvalue);
            effects.Add(tempE);
        }
        public SingleEffect getEffect(int No)
        {
            return effects[No];
        }
        public int getEffectNum()
        {
            return effects.Count;
        }

        public void copy(CraftEssence ce)
        {
            if (ce == null) return;
            ID = ce.ID;
            name = ce.name;
            rarity = ce.rarity;
            cost = ce.cost;
            ATK = ce.ATK;
            HP = ce.HP;
            maxLimit = ce.maxLimit;
            effects.Clear();
            for (int i = 0; i < ce.effects.Count; i++)
            {
                addEffect(ce.effects[i].targetType, ce.effects[i].type, ce.effects[i].value, ce.effects[i].turn, ce.effects[i].probability, ce.effects[i].afterNP, ce.effects[i].fullvalue);
            }

        }
    }
    public class Skill
    {
        private List<SingleEffect> effects;
        public int CD;
        public int leftCD;

        public Skill()
        {
            effects = new List<SingleEffect>();
            leftCD = 0;
        }

        public void addEffect(int targetType, int type, double value, int turn, double probability, bool afterNP)
        {
            SingleEffect tempE = new SingleEffect(targetType, type, value, turn, probability, afterNP);
            effects.Add(tempE);
        }
        public SingleEffect getEffect(int No)
        {
            return effects[No];
        }
        public int getEffectNum()
        {
            return effects.Count;
        }
        public void clearEffect()
        {
            effects.Clear();
        }

        public void use(Battlefield battlefield, int user, int target, bool afterNP, bool active)//使用技能，根据效果的对象，分配效果
        {
            for (int i = 0; i < effects.Count; i++)
            {
                if (battlefield.effectPossible(effects[i].probability))
                {
                    switch (effects[i].targetType)
                    {
                        case 0://无
                            {
                                //effects[i].effectHandle(battlefield, battlefield.allyServant[user], afterNP, active, false);
                                break;
                            }
                        case 1://自身
                            {
                                effects[i].effectHandle(battlefield, battlefield.allyServant[user], afterNP, active, false);
                                break;
                            }
                        case 2://单个队友
                            {
                                effects[i].effectHandle(battlefield, battlefield.allyServant[target], afterNP, active, false);
                                break;
                            }
                        case 3://全队
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    effects[i].effectHandle(battlefield, battlefield.allyServant[j], afterNP, active, false);
                                }
                                break;
                            }
                        case 4://除自己以外全队
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    if (j != user)
                                    {
                                        effects[i].effectHandle(battlefield, battlefield.allyServant[j], afterNP, active, false);
                                    }
                                }
                                break;
                            }
                        case 5://除单体以外全体
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    if (j != target)
                                    {
                                        effects[i].effectHandle(battlefield, battlefield.allyServant[j], afterNP, active, false);
                                    }
                                }
                                break;
                            }
                        case 6://敌方单体
                            {
                                effects[i].effectHandle(battlefield, battlefield.enemyServant[target], afterNP, active, false);
                                break;
                            }
                        case 7://敌方全体
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    effects[i].effectHandle(battlefield, battlefield.enemyServant[j], afterNP, active, false);
                                }
                                break;
                            }
                        default: break;
                    }
                }
            }
            leftCD = CD;
        }
        public bool canUse()
        {
            if (leftCD == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void copy(Skill s)
        {
            clearEffect();
            for (int i = 0; i < s.effects.Count; i++)
            {
                addEffect(s.effects[i].targetType, s.effects[i].type, s.effects[i].value, s.effects[i].turn, s.effects[i].probability, s.effects[i].afterNP);
            }
            CD = s.CD;
            leftCD = 0;
        }
        public void initCD()//马上释放状态
        {
            leftCD = 0;
        }

        public double getMOFANG(int type)//可能
        {
            double result = 0.0;
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i].type == type)
                {
                    result += effects[i].value;
                }
            }
            return result;
        }
    }
    public class SingleEffect
    {
        public int targetType;
        public int type;
        public double value;
        public double fullvalue;
        public int turn;
        public double probability;
        public bool afterNP;

        public SingleEffect(int iTargetType, int iType, double iValue, int iTurn, double iProbability, bool bAfter)//技能效果初始化
        {
            targetType = iTargetType;
            type = iType;
            value = iValue;
            fullvalue = iValue;
            turn = iTurn;
            probability = iProbability;
            afterNP = bAfter;
        }
        public SingleEffect(int iTargetType, int iType, double iValue, int iTurn, double iProbability, bool bAfter, double ifullval)//礼装效果初始化
        {
            targetType = iTargetType;
            type = iType;
            value = iValue;
            fullvalue = ifullval;
            turn = iTurn;
            probability = iProbability;
            afterNP = bAfter;
        }
        
        public void effectHandle(Battlefield battlefield, NowServant tar, bool after, bool erasable, bool maxlimit)//发动效果，根据效果种类，处理效果
        {
            double actualvalue = value;
            if (maxlimit) actualvalue = fullvalue;
            if (afterNP == after)
            {
                switch (type)
                //1 攻击力  2 np率  3 出星率  4 集星  5 宝具威力  678红蓝绿魔放  9暴击威力  10 防御下降  11 防御提升
                //12 攻击力下降  13 得np  14 得星  15 回血  161718战续闪避无敌 19 嘲讽  2021晕蓄力 22减CD
                {
                    case 0:
                        {
                            break;
                        }
                    case 13://回np
                        {
                            if (turn == 0)
                            {
                                tar.getnp(actualvalue);
                            }
                            else
                            {
                                tar.addBuff(type, actualvalue, turn, erasable);
                            }
                            break;
                        }
                    case 14://得星
                        {
                            if (turn == 0)
                            {
                                battlefield.nowStar += (int)actualvalue;
                            }
                            else
                            {
                                tar.addBuff(type, actualvalue, turn, erasable);
                            }
                            break;
                        }
                    case 22://减CD
                        {
                            tar.refreshSkill();
                            break;
                        }
                    default:
                        {
                            tar.addBuff(type, actualvalue, turn, erasable);
                            break;
                        }
                }
            }


        }
    }
    public class Memory//内存，储存着所有文件中读取来的从者和礼装
    {
        private List<Servant> servantList;
        private List<CraftEssence> craftessenceList;

        public Memory()
        {
            servantList = new List<Servant>();
            craftessenceList = new List<CraftEssence>();

            readServantList();
            readCraftessenceList();
        }

        private void readServantList()//从文件读取从者信息
        {
            string readfile = System.Windows.Forms.Application.StartupPath + "\\servant.txt";
            string[] strR = File.ReadAllLines(readfile);
            servantList.Clear();
            for (int i = 0; i < strR.Length; i++)
            {
                if (strR[i] != "")
                {
                    Servant tempServant = new Servant(strR[i]);
                    servantList.Add(tempServant);
                }
            }
        }
        private void readCraftessenceList()//从文件读取礼装信息
        {
            string readfile = System.Windows.Forms.Application.StartupPath + "\\craftessence.txt";
            string[] strR = File.ReadAllLines(readfile);
            craftessenceList.Clear();
            for (int i = 0; i < strR.Length; i++)
            {
                if (strR[i] != "")
                {
                    CraftEssence tempCE = new CraftEssence(strR[i]);
                    craftessenceList.Add(tempCE);
                }
            }
        }

        private int searchServant(Servant servant, int start, int end)//查找从者，返回值-1代表更新完毕，其他代表插入位置
        {
            if (servantList.Count == 0) return 0;

            if (servantList[(start + end) / 2].ID == servant.ID)
            {
                servantList[(start + end) / 2] = servant;
                return -1;
            }
            else
            {
                if (start == end)
                {
                    if (servantList[start].ID > servant.ID)
                    {
                        return start;
                    }
                    else
                    {
                        return start + 1;
                    }
                }
                else
                {
                    if (servantList[(start + end) / 2].ID > servant.ID)
                    {
                        return searchServant(servant, start, (start + end) / 2);
                    }
                    else
                    {
                        return searchServant(servant, (start + end) / 2 + 1, end);
                    }
                }
            }
        }
        private int searchCE(CraftEssence ce, int start, int end)//查找礼装，返回值-1代表更新完毕，其他代表插入位置
        {
            if (craftessenceList.Count == 0) return 0;

            if (craftessenceList[(start + end) / 2].ID == ce.ID)
            {
                craftessenceList[(start + end) / 2] = ce;
                return -1;
            }
            else
            {
                if (start == end)
                {
                    if (craftessenceList[start].ID > ce.ID)
                    {
                        return start;
                    }
                    else
                    {
                        return start + 1;
                    }
                }
                else
                {
                    if (craftessenceList[(start + end) / 2].ID > ce.ID)
                    {
                        return searchCE(ce, start, (start + end) / 2);
                    }
                    else
                    {
                        return searchCE(ce, (start + end) / 2 + 1, end);
                    }
                }
            }
        }
        public void addServant(Servant saveServant)//将某个从者信息更新进内存和文件
        {
            int searchResult = searchServant(saveServant, 0, servantList.Count - 1);
            if (searchResult > -1)
            {
                servantList.Insert(searchResult, saveServant);
            }

            string writefile = System.Windows.Forms.Application.StartupPath + "\\servant.txt";
            FileStream fs = new FileStream(writefile, FileMode.Create, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fs);
            for (int i = 0; i < servantList.Count; i++)
            {
                sr.WriteLine(servantList[i].writeS());
            }
            sr.Close();
            fs.Close();
        }
        public void addCE(CraftEssence saveCE)//将某个礼装信息更新进内存和文件
        {
            int searchResult = searchCE(saveCE, 0, craftessenceList.Count - 1);

            if (searchResult > -1)
            {
                craftessenceList.Insert(searchResult, saveCE);
            }
            string writefile = System.Windows.Forms.Application.StartupPath + "\\craftessence.txt";
            FileStream fs = new FileStream(writefile, FileMode.Create, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fs);
            for (int i = 0; i < craftessenceList.Count; i++)
            {
                sr.WriteLine(craftessenceList[i].writeCE());
            }
            sr.Close();
            fs.Close();
        }

        public void refreshComboBox(ComboBox combobox)//将从者显示在combobox里
        {
            combobox.Items.Clear();
            for (int i = 0; i < servantList.Count; i++)
            {
                combobox.Items.Add(servantList[i].ID + " " + servantList[i].name);
            }
        }
        public void refreshCEComboBox(ComboBox combobox)//将礼装显示在combobox里
        {
            combobox.Items.Clear();
            for (int i = 0; i < craftessenceList.Count; i++)
            {
                combobox.Items.Add(craftessenceList[i].ID + " " + craftessenceList[i].name);
            }
        }
        public void showComboBox(ComboBox combobox, int ID)//替从者combobox选一个
        {
            if (servantList.Count > 0)
            {
                for (int i = 0; i < servantList.Count; i++)
                {
                    if (servantList[i].ID == ID)
                    {
                        combobox.Text = servantList[i].ID + " " + servantList[i].name;
                        return;
                    }
                }
            }
            if (servantList.Count > 0)
            {
                combobox.Text = servantList[0].ID + " " + servantList[0].name;
            }
        }

        public Servant getServantWithComboText(string ComboText)//从ID得到从者
        {
            string[] sArray = ComboText.Split(' ');
            int ID = int.Parse(sArray[0]);
            int No = -1;
            for (int i = 0; i < servantList.Count; i++)
            {
                if (ID == servantList[i].ID)
                {
                    No = i;
                }
            }
            if (No == -1) return null;
            return servantList[No];
        }
        public CraftEssence getCEWithComboText(string ComboText)//从ID得到礼装
        {
            string[] sArray = ComboText.Split(' ');
            if (sArray[0] == "") return null;
            int ID = int.Parse(sArray[0]);
            int No = -1;
            for (int i = 0; i < craftessenceList.Count; i++)
            {
                if (ID == craftessenceList[i].ID)
                {
                    No = i;
                }
            }
            if (No == -1) return null;
            return craftessenceList[No];
        }

    }

    public class NowServant : Servant//附带额外的场面信息
    {
        public double nownp;
        public double sumnp;
        public double sumAdmg;
        public double sumNPdmg;
        public int sumCrestar;
        public int sumGetstar;
        public bool specialATK;

        public List<Buff> buffs;
        public CraftEssence equippedCE;

        public NowServant()
        {
            buffs = new List<Buff>();
            equippedCE = new CraftEssence();
        }

        public void copyS(Servant s)
        {
            ID = s.ID;
            name = s.name;
            rarity = s.rarity;
            servantClass = s.servantClass;
            cost = s.cost;

            ATK = s.ATK;
            HP = s.HP;
            attribute = s.attribute;
            npCharge = s.npCharge;
            npChargeDEF = s.npChargeDEF;
            starGe = s.starGe;
            starAb = s.starAb;

            redCards = s.redCards;
            redHits = s.redHits;
            blueCards = s.blueCards;
            blueHits = s.blueHits;
            greenCards = s.greenCards;
            greenHits = s.greenHits;
            EXHits = s.EXHits;

            NPtype = s.NPtype;
            NPdmg = s.NPdmg;
            NPHits = s.NPHits;
            NPtarget = s.NPtarget;

            for (int i = 0; i < 5; i++)
            {
                skills[i].copy(s.skills[i]);
            }
        }
        public void refresh()//战场数据初始化
        {
            nownp = 0.0;
            sumnp = 0.0;
            sumAdmg = 0.0;
            sumNPdmg = 0.0;
            sumCrestar = 0;
            sumGetstar = 0;
            specialATK = false;

            buffs.Clear();
            for (int i = 0; i < 4; i++)
            {
                skills[i].initCD();
            }
        }
        public bool NPavailable()//是否可以使用宝具
        {
            /*
            for (int i = 0; i < 3; i++)
            {
                if (skills[i].getMOFANG(NPtype + 5) >= 0.5 && skills[i].leftCD == 1)////可能
                {
                    return false;
                }
            }
            */
            return (Math.Round(nownp) >= 100.0);
        }
        public void getnp(double np)//增加np
        {
            nownp += np;
            sumnp += np;
        }
        public bool disable(List<int> type)//是否有某些不能行动状态
        {
            bool result = false;
            for (int j = 0; j < type.Count; j++)
            {
                for (int i = 0; i < buffs.Count; i++)
                {
                    if (buffs[i].type == type[j])
                    {
                        if (buffs[i].value == 1)//一直生效
                        {
                            return true;
                        }
                        if (buffs[i].value == 2 && buffs[i].leftTurn==1)//最后一回合生效
                        {
                            return true;
                        }
                    }
                }
            }
            return result;
        }
        public void addBuff(int type, double value, int turn, bool erasable)
        {
            if (!erasable || turn != 0)
            {
                Buff tempbuff = new Buff(type, value, turn);
                buffs.Add(tempbuff);
            }
        }
        public int refreshBuff()//buff效果处理剩余回合减一，默认返回出星
        {
            int star = 0;
            for (int i = buffs.Count - 1; i >= 0; i--)
            {
                switch (buffs[i].type)
                {
                    //1 攻击力  2 np率  3 出星率  4 集星  5 宝具威力  678红蓝绿魔放  9暴击威力  10 防御下降  11 防御提升  12 攻击力下降  13 得np  14 得星  15 回血  161718战续闪避无敌 19 嘲讽  2021晕蓄力
                    case 13:
                        {
                            this.getnp(buffs[i].value);
                            break;
                        }
                    case 14:
                        {
                            star += (int)buffs[i].value;
                            break;
                        }

                    default: break;
                }

                if (buffs[i].leftTurn > 0)
                {
                    buffs[i].leftTurn--;
                    if (buffs[i].leftTurn == 0)
                    {
                        buffs.Remove(buffs[i]);
                    }
                }
                else if (buffs[i].leftTurn < -1)//可能：次数暂时
                {
                    buffs.Remove(buffs[i]);
                }
            }
            return star;
        }
        public double getBuff(int type)
        {
            double result = 0.0;
            for (int i = 0; i < buffs.Count; i++)
            {
                if (buffs[i].type == type)
                {
                    result += buffs[i].value;
                }
            }
            return result;
        }
        public void refreshSkill()//技能剩余CD减一
        {
            for (int i = 0; i < 3; i++)
            {
                if (skills[i].leftCD != 0)
                {
                    skills[i].leftCD--;
                }
            }
        }


    }
    public class Buff
    {
        public int type;
        public double value;
        public int leftTurn;//0为永久，负数则为次数

        public Buff(int ty, double val, int tu)
        {
            type = ty;
            value = val;
            leftTurn = tu;
        }
    }
    public class Hit//储存着某一击的所有信息
    {
        public int attackerNo;
        public int attackedNo;
        public int hitType;
        public int hitTimes;
        public int starAb;
        public bool isCrit;
        public bool isNP;

        public bool used;

        public double getnp;
        public int getStar;
        public double MakeAdmg;
        public double MakeNPdmg;


        public Hit()
        {

        }
        public Hit(Hit copy)
        {
            attackerNo = copy.attackerNo;
            attackedNo = copy.attackedNo;
            hitType = copy.hitType;
            hitTimes = copy.hitTimes;
            starAb = copy.starAb;
            isCrit = copy.isCrit;
            isNP = copy.isNP;

            used = false;
            getnp = 0;
            getStar = 0;
            MakeAdmg = 0;
            MakeNPdmg = 0;
        }

        public Hit(Servant attacker, int erNo, int edNo, int ihitT, bool bisNP)//生成一hit
        {
            attackerNo = erNo;
            attackedNo = edNo;
            hitType = ihitT;
            if (bisNP)
            {
                hitTimes = attacker.NPHits;
                if (attacker.NPtarget == 3)
                {
                    attackedNo = 3;
                }
            }
            else
            {
                switch (hitType)
                {
                    case 0:
                        {
                            hitTimes = attacker.EXHits;
                            break;
                        }
                    case 1:
                        {
                            hitTimes = attacker.redHits;
                            break;
                        }
                    case 2:
                        {
                            hitTimes = attacker.blueHits;
                            break;
                        }
                    case 3:
                        {
                            hitTimes = attacker.greenHits;
                            break;
                        }
                }
            }
            starAb = attacker.starAb;
            isCrit = false;
            isNP = bisNP;

            used = false;
            getnp = 0;
            getStar = 0;
            MakeAdmg = 0;
            MakeNPdmg = 0;
        }
    }
    public class ResultData//每回合得到的一组数据
    {
        //public bool EXexist;
        //public int chaintype;

        public List<Hit> candidateHits;
        public List<Hit> pickedHits;
    }
    public class Battlefield//战斗场地，一切和场面战斗相关的操作
    {
        Random ran = new Random();

        public Form1 formMain;

        public NowServant[] allyServant;//场上3个从者
        public int servantNum;//场上几个从者

        public NowServant[] enemyServant;
        public int targetNum;//有几个目标

        public int turnNum;//测试的第几回合
        public int nowStar;//现有星星

        List<Hit> waitingHits;//存放运算出的随机卡片
        List<Hit> candidateHits;//5张候选卡片和宝具和EX
        List<Hit> pickedHits;//选择的3张卡片

        public int[] strategy;//18,6个为一角色,0-2为释放对象,3-5为释放策略
        public int[] priority;//3*3+4行动卡优先级，前三组为0-2行动卡，最后一组为环境chain优先级
        public int[] NPpriority;//3个宝具优先级


        const int MaxServant = 3;//最多从者数
        const int pickedNum = 3;//选择的卡牌数量
        const int chainNum = 3;//chain和EX需要的卡牌数量
        const int selectedNum = 5;//可供选择的卡牌数量
        const int skillNum = 3;//技能总数
        const int MaxStar = 10;//卡片最多得到星星

        public Battlefield(Form1 form)
        {
            formMain = form;

            allyServant = new NowServant[MaxServant];
            enemyServant = new NowServant[MaxServant];
            for (int i = 0; i < MaxServant; i++)
            {
                allyServant[i] = new NowServant();
                enemyServant[i] = new NowServant();
            }

            waitingHits = new List<Hit>();

            candidateHits = new List<Hit>();

            pickedHits = new List<Hit>();

            strategy = new int[18];
            priority = new int[16];
            NPpriority = new int[3];
        }
        
        public void initPassiveSkill()//被动技能buff
        {
            for (int i = 0; i < servantNum; i++)
            {
                allyServant[i].skills[skillNum].use(this, i, i, false, false);
            }
        }
        public void initCE(bool[] maxlimit)//礼装buff
        {

            for (int j = 0; j < servantNum; j++)
            {
                //非满破默认只加礼装0.4HP ATK
                double maxlimitMod = 1;
                if (!maxlimit[j]) maxlimitMod = 0.4;
                allyServant[j].ATK += (int)(allyServant[j].equippedCE.ATK * maxlimitMod);
                allyServant[j].HP += (int)(allyServant[j].equippedCE.HP * maxlimitMod);
                for (int i = 0; i < allyServant[j].equippedCE.getEffectNum(); i++)
                {
                    switch (allyServant[j].equippedCE.getEffect(i).targetType)
                    {
                        case 1://自身
                            {
                                allyServant[j].equippedCE.getEffect(i).effectHandle(this, allyServant[j], false, false, maxlimit[j]);
                                break;
                            }
                        case 3://全队
                            {
                                for (int q = 0; q < servantNum; q++)
                                {
                                    allyServant[j].equippedCE.getEffect(i).effectHandle(this, allyServant[q], false, false, maxlimit[j]);
                                }
                                break;
                            }
                        default: break;
                    }
                }
            }
        }

        public void refresh()//初始化信息
        {
            turnNum = 0;
            nowStar = 0;
            waitingHits.Clear();
            candidateHits.Clear();
            pickedHits.Clear();

            for (int i = 0; i < servantNum; i++)
            {
                allyServant[i].refresh();//审查
            }
        }

        public void setNN(int serventN, int targetN)//设置战场人数
        {
            servantNum = serventN;
            targetNum = targetN;
        }
        
        public ResultData oneTurn()
        {
            turnNum++;
            pickedHits.Clear();
            formMain.skillClear();
            ResultData result = new ResultData();

            //计算buff，并到期
            buffStage();

            //刷新技能CD
            skillCDStage();

            //释放技能
            skillStage();

            //展示行动卡
            hitCreateStage();

            //选择AI：先按优先级从大到小查看特殊情况，每个特殊情况中对卡片再从大到小进行挑选
            List<int> pickedNo = new List<int>();
            AIstage(pickedNo);

            //分配星星
            starDeliverStage();

            //确定行动卡，优化顺序
            confirmStage(result,pickedNo);

            //释放技能
            skillStage();

            //我方攻击
            battleCalStage();

            //敌方攻击
            enemybattleCalStage();

            return result;
        }

        private void buffStage()//计算buff，并到期
        {
            for (int i = 0; i < servantNum; i++)
            {
                nowStar += allyServant[i].refreshBuff();//审查
            }
        }
        private void skillCDStage()//刷新技能CD
        {
            for (int i = 0; i < servantNum; i++)
            {
                allyServant[i].refreshSkill();//审查
            }
            for (int i = 0; i < targetNum; i++)
            {
                enemyServant[i].refreshBuff();
            }
        }
        private void hitCreateStage()//完成行动卡的生成和最新5张的置后
        {
            //生成新一轮行动卡
            if (waitingHits.Count == 0)
            {
                for (int i = 0; i < servantNum; i++)
                {
                    for (int j = 0; j < allyServant[i].redCards; j++)
                    {
                        waitingHits.Add(new Hit(allyServant[i], i, 0, 1, false));
                    }
                    for (int j = 0; j < allyServant[i].blueCards; j++)
                    {
                        waitingHits.Add(new Hit(allyServant[i], i, 0, 2, false));
                    }
                    for (int j = 0; j < allyServant[i].greenCards; j++)
                    {
                        waitingHits.Add(new Hit(allyServant[i], i, 0, 3, false));
                    }
                }
            }

            //随机选择5张行动卡并将其置后
            for (int i = 0; i < selectedNum; i++)
            {
                int targetNo = waitingHits.Count - i;
                int nowNo = NumPossible(1, targetNo);
                //前targetNo-1个中随机一个和第targetNo个地址互换
                Hit temphit = waitingHits[nowNo - 1];
                waitingHits[nowNo - 1] = waitingHits[targetNo - 1];
                waitingHits[targetNo - 1] = temphit;
            }

            // 将最新5张以及可能的宝具放入候选卡
            candidateHits.Clear();
            for (int i = 0; i < selectedNum; i++)
            {
                candidateHits.Add(new Hit(waitingHits[waitingHits.Count - 1]));
                waitingHits.RemoveAt(waitingHits.Count - 1);
            }
            for (int i = 0; i < servantNum; i++)
            {
                if (Math.Round(allyServant[i].nownp) >= 100.0)
                {
                    candidateHits.Add(new Hit(allyServant[i], i, 0, allyServant[i].NPtype, true));
                }
            }
        }
        private void AIstage(List<int> pickedNo)//选择AI：先按优先级从大到小查看特殊情况，每个特殊情况中对卡片再从大到小进行挑选
        {
            int[] chainPriority = new int[5+servantNum];//chain优先级表 0-3红蓝绿EX 4为XJB
            for (int i = 0; i < 4; i++)
            {
                chainPriority[i] = priority[3 * 3 + i];
            }
            chainPriority[4] = 10;//XJB优先级
            for (int i = 5; i<candidateHits.Count; i++)
            {
                chainPriority[candidateHits[i].attackerNo+5] = NPpriority[candidateHits[i].attackerNo];
            }

            int[] cardPriority = new int[selectedNum];//候选卡的优先级表
            for (int i = 0; i < selectedNum; i++)
            {
                cardPriority[i] = priority[3 * candidateHits[i].attackerNo + candidateHits[i].hitType - 1];
            }

            int needToPick = 3;
            bool isSuccess = false;
            List<int> NPlist = new List<int>();
            for (int i = 0; i < 5 && !isSuccess; i++)//对chain优先级从大到小进行考察
            {
                int nowtype = maxPriority(chainPriority, -1);
                if (nowtype == 3)//EX
                {
                    int temp = checkEX(candidateHits, NPlist);
                    if (temp > -1)
                    {
                        for (int j = 0; j < selectedNum; j++)
                        {
                            if (candidateHits[j].attackerNo != temp)//非本人的优先级归0
                            {
                                cardPriority[j] = 0;
                            }
                        }
                        for (int j = 0; j < needToPick; j++)//以优先级选3张卡
                        {
                            int nowCard = maxPriority(cardPriority, -1);
                            pickedNo.Add(nowCard);
                        }
                        isSuccess = true;
                    }
                }
                else if (nowtype < 3)//红蓝绿chain
                {
                    if (checkChain(candidateHits, nowtype + 1) > 0) //非本颜色的优先级归0
                    {
                        for (int j = 0; j < selectedNum; j++)
                        {
                            if (candidateHits[j].hitType != nowtype + 1)
                            {
                                cardPriority[j] = 0;
                            }
                        }
                        for (int j = 0; j < needToPick; j++)//以同色优先级选3张卡
                        {
                            int nowCard = maxPriority(cardPriority, nowtype + 1);
                            pickedNo.Add(nowCard);
                        }
                        isSuccess = true;
                    }
                }
                else if (nowtype == 4)//XJB
                {
                    for (int j = 0; j < needToPick; j++)
                    {
                        int nowCard = maxPriority(cardPriority, -1);
                        pickedNo.Add(nowCard);
                    }
                    isSuccess = true;
                }
                else//开宝具
                {
                    NPlist.Add(nowtype - 5);
                    for (int j = 5; j < candidateHits.Count; j++)
                    {
                        if (candidateHits[j].attackerNo == nowtype-5)
                        {
                            pickedNo.Add(j);
                            break;
                        }
                    }
                    if (allyServant[nowtype-5].NPtype == 1)
                    {
                        chainPriority[1] = 0;
                        chainPriority[2] = 0;
                    }
                    if (allyServant[nowtype-5].NPtype == 2)
                    {
                        chainPriority[0] = 0;
                        chainPriority[2] = 0;
                    }
                    if (allyServant[nowtype-5].NPtype == 3)
                    {
                        chainPriority[0] = 0;
                        chainPriority[1] = 0;
                    }
                    i--;
                    needToPick--;
                }
            }
            
        }
        private void starDeliverStage()//分配星星
        {
            //集星buff
            for (int i = 0; i < selectedNum; i++)
            {
                candidateHits[i].starAb = (int)(candidateHits[i].starAb * (1 + allyServant[candidateHits[i].attackerNo].getBuff(formMain.getNoFromName(formMain.effectName, "集星"))));
                if (candidateHits[i].starAb < 0) candidateHits[i].starAb = 0;
            }

            //随机分配50 20 20
            bool[] used = new bool[selectedNum];
            for (int i = 0; i < selectedNum; i++)
            {
                used[i] = false;
            }
            for (int i = 0; i < 3; i++)
            {
                int addAb = (i == 0 ? 50 : 20);
                while (true)
                {
                    int temp = NumPossible(1, selectedNum) - 1;
                    if (!used[temp])
                    {
                        used[temp] = true;
                        candidateHits[temp].starAb += addAb;
                        break;
                    }
                }
            }

            //随机给星
            //将概率分层，最后效果俄罗斯转盘
            int sumAb = 0;
            int[] levelAb = new int[selectedNum];
            for (int i = 0; i < selectedNum; i++)
            {
                sumAb += candidateHits[i].starAb;
                levelAb[i] = sumAb;
            }
            while (nowStar != 0 && levelAb[selectedNum - 1] != 0)
            {
                int ranLevel = NumPossible(1, levelAb[selectedNum - 1]);
                for (int j = 0; j < selectedNum; j++)
                {
                    if (ranLevel <= levelAb[j])
                    {
                        candidateHits[j].getStar++;
                        nowStar--;
                        if (candidateHits[j].getStar == MaxStar)
                        {
                            int tempMinus;
                            if (j == 0)
                            {
                                tempMinus = levelAb[j];
                            }
                            else
                            {
                                tempMinus = levelAb[j] - levelAb[j - 1];
                            }
                            for (int i = j; i < selectedNum; i++)
                            {
                                levelAb[i] -= tempMinus;
                            }
                        }
                        break;
                    }
                }
            }
            nowStar = 0;

            //计算实际暴击了没
            for (int i = 0; i < selectedNum; i++)
            {
                candidateHits[i].isCrit = critPossible(candidateHits[i].getStar);
            }
        }
        private void confirmStage(ResultData result, List<int> pickedNo)//确定行动卡
        {
            //先把宝具搬到优先级低的地方，类似冒泡
            bool isFinish = false;
            
            for (int i = pickedNum - 2; i >= 0; i--)
            {
                for (int j = i; j < pickedNum - 1; j++)
                {
                    if (candidateHits[pickedNo[j]].isNP)
                    {
                        if (!candidateHits[pickedNo[j + 1]].isNP )
                        {
                            if (candidateHits[pickedNo[j + 1]].hitType >= candidateHits[pickedNo[j]].hitType)
                            {
                                int temp = pickedNo[j];
                                pickedNo[j] = pickedNo[j + 1];
                                pickedNo[j + 1] = temp;
                                isFinish = true;
                            }
                        }
                    }
                }
            }
            
            //将优先级总体来说逆序输入pickedhits，优先级高的在后面
            for (int i = pickedNum - 1; i >= 0 && !isFinish; i--) //有红卡，那一定红卡打头
            {
                if (candidateHits[pickedNo[i]].hitType == 1 )
                {
                    for (int j = i; j < pickedNum - 1; j++)
                    {
                        int temp = pickedNo[j];
                        pickedNo[j] = pickedNo[j+1];
                        pickedNo[j+1] = temp;
                    }
                    isFinish = true;
                }
            }
            if (!isFinish)
            {
                for (int i = pickedNum - 1; i >= 0 && !isFinish; i--)//没红卡，找蓝卡打头
                {
                    if (candidateHits[pickedNo[i]].hitType == 2)
                    {
                        int temp = pickedNo[i];
                        pickedNo[i] = pickedNo[pickedNum - 1];
                        pickedNo[pickedNum - 1] = temp;
                        isFinish = true;
                    }
                }
            }

            pickedHits.Clear();
            for (int i = pickedNum-1; i >=0; i--)
            {
                pickedHits.Add(new Hit(candidateHits[pickedNo[i]]));
            }
            result.candidateHits = candidateHits;
            result.pickedHits = pickedHits;
        }
        private void skillStage()
        {
            for (int j = 0; j < servantNum; j++)
            {
                for (int i = 0; i < skillNum; i++)
                {
                    if (allyServant[j].skills[i].canUse())
                    {
                        switch (formMain.strategyName[strategy[6 * j + 3 + i]])
                        {
                            //"立即","满np不用","绑定宝具", "绑定红爆", "绑定蓝卡"
                            case "立即":
                                {
                                    allyServant[j].skills[i].use(this, j, strategy[6 * j + i], false, true);formMain.skillUsed(j, i);
                                    break;
                                }
                            case "满np不用":
                                {
                                    if (!allyServant[strategy[6 * j + i]].NPavailable())
                                    {
                                        allyServant[j].skills[i].use(this, j, strategy[6 * j + i], false, true);
                                        formMain.skillUsed(j, i);
                                    }
                                    break;
                                }
                            case "绑定宝具":
                                {
                                    if (checkCardType(pickedHits, strategy[6 * j + i], 4, false))
                                    {
                                        allyServant[j].skills[i].use(this, j, strategy[6 * j + i], false, true);
                                        formMain.skillUsed(j, i);
                                    }
                                    break;
                                }
                            case "绑定红爆":
                                {
                                    if (checkCardType(pickedHits, strategy[6 * j + i], 1, true))
                                    {
                                        allyServant[j].skills[i].use(this, j, strategy[6 * j + i], false, true);
                                        formMain.skillUsed(j, i);
                                    }
                                    break;
                                }
                            case "绑定蓝卡":
                                {
                                    if (checkCardType(pickedHits, strategy[6 * j + i], 2, false))
                                    {
                                        allyServant[j].skills[i].use(this, j, strategy[6 * j + i], false, true);
                                        formMain.skillUsed(j, i);
                                    }
                                    break;
                                }
                            default: break;
                        }

                    }
                }
            }
        }
        private void battleCalStage()//蓝绿chain EX 计算阶段:宝具buff，宝具np，计算伤害，宝具后buff
        {
            //蓝绿chain EX
            if (checkChain(pickedHits, 2) > 0)//蓝
            {
                bool[] used = new bool[3] { false, false, false };
                for (int i = 0; i < pickedNum; i++)
                {
                    if (!used[pickedHits[i].attackerNo])
                    {
                        used[pickedHits[i].attackerNo] = true;
                        allyServant[pickedHits[i].attackerNo].getnp(20.0);
                    }
                }
            }
            if (checkChain(pickedHits, 3) > 0)//绿
            {
                nowStar += 10;
            }
            List<int> emptylist = new List<int>();
            int EXresult = checkEX(pickedHits, emptylist);
            if (EXresult > -1) //EX就把candidate的白卡加进来
            {
                pickedHits.Add(new Hit(allyServant[EXresult], EXresult, 0, 0, false));
            }

            //计算阶段:宝具buff，宝具np，计算伤害，宝具后buff
            for (int i = 0; i < pickedHits.Count; i++)
            {
                List<int> stunType =new List<int>();
                stunType.Add(formMain.getNoFromName(formMain.effectName,"眩晕"));
                if (allyServant[pickedHits[i].attackerNo].disable(stunType)) continue;

                //宝具先走buff及NP清零
                if (pickedHits[i].isNP)
                {
                    allyServant[pickedHits[i].attackerNo].skills[4].use(this, pickedHits[i].attackerNo, 0, false, true);//审查
                    allyServant[pickedHits[i].attackerNo].nownp = 0;
                }

                for (int j = 0; j < targetNum; j++)
                {
                    if (pickedHits[i].attackedNo == 3 || pickedHits[i].attackedNo == j)
                    {


                        //中间计算
                        double np = calculateNp(i, j);
                        allyServant[pickedHits[i].attackerNo].getnp(np);
                        pickedHits[i].getnp += np;

                        double dmg = calculateDmg(i, j);
                        if (pickedHits[i].isNP)
                        {
                            allyServant[pickedHits[i].attackerNo].sumNPdmg += dmg;
                            pickedHits[i].MakeNPdmg += dmg;
                        }
                        else
                        {
                            allyServant[pickedHits[i].attackerNo].sumAdmg += dmg;
                            pickedHits[i].MakeAdmg += dmg;
                        }

                        int star = calculateStar(i, j);
                        nowStar += star;
                        pickedHits[i].getStar += star;
                    }


                }
                //宝具后走buff
                if (pickedHits[i].isNP)
                {
                    allyServant[pickedHits[i].attackerNo].skills[4].use(this, pickedHits[i].attackerNo, 0, true, true);
                }

            }

        }
        private void enemybattleCalStage()//敌方攻击
        {
            for (int i = 0; i < 3; i++)//3次攻击
            {
                double hitTimes = 1.0;
                int target = NumPossible(1, servantNum) - 1;
                allyServant[target].getnp(allyServant[target].npChargeDEF * hitTimes);
            }
        }

        //概率类函数
        private bool critPossible(int getStar)
        {
            int RandKey = ran.Next(1, 10 + 1);
            if (RandKey <= getStar)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool effectPossible(double probability)
        {
            return (ran.NextDouble() < probability);
        }
        private int NumPossible(int smallM, int bigM)// return random of [smallM,+1,+2,..,big] 
        {
            int RandKey = ran.Next(smallM, bigM + 1);
            return RandKey;
        }

        //计算类函数
        private double calculateNp(int No, int attackedNo)
        {
            double Np = 0.0;
            NowServant thisServant = allyServant[pickedHits[No].attackerNo];
            NowServant attackedServant = enemyServant[attackedNo];

            double npCharge = thisServant.npCharge;
            double firstblue = (pickedHits[0].hitType == 2 && !pickedHits[No].isNP) ? 1.0 : 0;//首蓝加成，宝具不吃
            double[] cardNpValueList = new double[4] { 1.0, 0.0, 3.0, 1.0 };
            double cardNpValue = cardNpValueList[pickedHits[No].hitType];//颜色加成
            double npOrderValue = (pickedHits[No].hitType != 0 && !pickedHits[No].isNP) ? (1.0 + 0.5 * No) : 1.0;//位置加成，宝具EX不吃
            double cardMod = 0.0;//卡牌魔放buff
            if (pickedHits[No].hitType == 1) cardMod = thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "红魔放"));
            if (pickedHits[No].hitType == 2) cardMod = thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "蓝魔放"));
            if (pickedHits[No].hitType == 3) cardMod = thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "绿魔放"));

            double npChargeMod = thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "np率")); ;//NP率buff
            double criticalModifier = (pickedHits[No].isCrit) ? 2.0 : 1.0;//2 when critical
            double overkillModifier = 1.0;//1.5 when overkill
            Np += pickedHits[No].hitTimes * npCharge * (firstblue + (cardNpValue * npOrderValue * (1 + cardMod))) * (1 + npChargeMod) * criticalModifier * overkillModifier;

            return Np;
        }
        private double calculateDmg(int No, int attackedNo)
        {
            if (pickedHits[No].hitTimes == 0) return 0;
            double DMG = 0.0;
            NowServant thisServant = allyServant[pickedHits[No].attackerNo];
            NowServant attackedServant = enemyServant[attackedNo];

            const double AtkModifier = 0.23;
            double ATK = thisServant.ATK;
            double NPdmgMultiplier = (pickedHits[No].isNP) ? thisServant.NPdmg / 100.0 : 1.0;//宝具倍率
            double firstred = (pickedHits[0].hitType == 1 && !pickedHits[No].isNP) ? 0.5 : 0;//首红加成，宝具不吃
            double[] cardDamageValueList = new double[4] { 1.0, 1.5, 1.0, 0.8 };//颜色加成
            double cardDamageValue = cardDamageValueList[pickedHits[No].hitType];
            double dmgOrderValue = (pickedHits[No].hitType != 0 && !pickedHits[No].isNP) ? (1.0 + 0.2 * No) : 1.0;//位置加成，宝具EX不吃            
            double cardMod = 0.0;//卡牌魔放buff
            if (pickedHits[No].hitType == 1) cardMod = thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "红魔放"))- attackedServant.getBuff(formMain.getNoFromName(formMain.effectName, "红魔放"));
            if (pickedHits[No].hitType == 2) cardMod = thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "蓝魔放")) - attackedServant.getBuff(formMain.getNoFromName(formMain.effectName, "蓝魔放"));
            if (pickedHits[No].hitType == 3) cardMod = thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "绿魔放")) - attackedServant.getBuff(formMain.getNoFromName(formMain.effectName, "绿魔放"));
            
            double classAtkBonus = formMain.classATKbonusList[thisServant.servantClass];//职介补正
            double triangleModifier = calculateTriangleModifier(pickedHits[No].attackerNo, attackedNo);//职介相性补正
            double attributeModifier = calculateAttributeModifier(pickedHits[No].attackerNo, attackedNo);//阵营相性补正
            double randomModifier = 1.0;//随机系数
            double AtkMod = thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "攻击力"));//攻击buff
            double DefMod = enemyServant[attackedNo].getBuff(formMain.getNoFromName(formMain.effectName, "防御力"));//防御buff
            double criticalModifier = (pickedHits[No].isCrit) ? 2.0 : 1.0; //2 when critical
            double extraCardModifier = 1.0;// EX卡有2.0加成，有chain则3.5
            if (pickedHits[No].hitType == 0)
            {
                if (checkChain(pickedHits, 1) > 0 || checkChain(pickedHits, 2) > 0 || checkChain(pickedHits, 3) > 0)
                { extraCardModifier = 3.5; }
                else
                { extraCardModifier = 2.0; }
            }

            double specialDefMod = 0.0;//特防buff
            double powerMod = (thisServant.specialATK) ? thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "特攻")) : 0.0;//特攻     

            double critDamageMod = (pickedHits[No].isCrit) ? thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "爆伤")) : 0.0;//爆伤威力buff
            double npDamageMod = (pickedHits[No].isNP) ? thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "宝具威力")) : 0.0;//宝具威力
            double superEffectiveModifier = (pickedHits[No].isNP && thisServant.specialATK) ? thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "宝具特攻")) : 0.0;//宝具特攻
            double dmgPlusAdd = thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "额外伤害"));//额外伤害
            double busterChainMod = (checkChain(pickedHits, 1) > 0 && !pickedHits[No].isNP) ? 0.2 : 0.0;// {0.2 if it's a Buster card in a Buster Chain, 0 otherwise}

            DMG = (ATK * NPdmgMultiplier * (firstred + (cardDamageValue * dmgOrderValue * (1.0 + cardMod))) * classAtkBonus * triangleModifier * attributeModifier *
                randomModifier * AtkModifier * (1.0 + AtkMod - DefMod) * criticalModifier * extraCardModifier
                * (1.0 - specialDefMod) * (1.0 + powerMod + critDamageMod + npDamageMod) * (superEffectiveModifier + 1.0)) + dmgPlusAdd + (ATK * busterChainMod);

            return DMG;
        }
        private int calculateStar(int No, int attackedNo)
        {
            int Star = 0;
            NowServant thisServant = allyServant[pickedHits[No].attackerNo];
            NowServant attackedServant = enemyServant[attackedNo];

            double starGe = thisServant.starGe / 100.0;
            double firstgreen = (pickedHits[0].hitType == 3 && !pickedHits[No].isNP) ? 0.2 : 0;//首绿加成，宝具不吃
            double[] cardStarValueList = new double[4] { 1.0, 0.1, 0.0, 0.8 };
            double cardStarValue = cardStarValueList[pickedHits[No].hitType];//颜色加成
            double starOrderValue = (pickedHits[No].hitType != 0 && !pickedHits[No].isNP) ? (1.0 + 0.5 * No) : 1.0;//位置加成，宝具EX不吃
            double cardMod = 0.0;//卡牌魔放buff
            if (pickedHits[No].hitType == 1) cardMod = thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "红魔放"));
            if (pickedHits[No].hitType == 2) cardMod = thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "蓝魔放"));
            if (pickedHits[No].hitType == 3) cardMod = thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "绿魔放"));
            double starDropMod = thisServant.getBuff(formMain.getNoFromName(formMain.effectName, "出星率")); ;//出星率buff
            double criticalModifier = (pickedHits[No].isCrit) ? 0.2 : 0;//0.2 when critical
            double overkillModifier = 1.0;//1.0 when overkill
            double overkillAdd = 0.0;//0.3 when overkill
            Star += (int)Math.Round(pickedHits[No].hitTimes * (starGe + firstgreen + (cardStarValue * starOrderValue * (1 + cardMod)) + starDropMod + criticalModifier) * overkillModifier + overkillAdd);

            return Star;
        }
        public double calculateNPdmg(int servantNo,int NPlevel)//计算宝具等级后的倍率
        {
            double result = 0.0;
            if (allyServant[servantNo].NPdmg != 0)
            {
                double[] NPdmgMod = new double[5] { 0, 4, 6, 7, 8 };
                double[] NPdmgModValue = new double[6] { 50, 25, 75, 37.5, 100, 50 };//对应红单群蓝单群绿单群
                int temp = 0;
                temp += 2 * (allyServant[servantNo].NPtype - 1);
                if (allyServant[servantNo].NPtarget != 1) temp++;

                result = allyServant[servantNo].NPdmg + NPdmgMod[NPlevel-1] * NPdmgModValue[temp];
            }
            return result;
        }
        public double calculateTriangleModifier(int attackerNo, int attackedNo)//职介克制
        {
            double result = 1.0;
            switch (formMain.className[allyServant[attackerNo].servantClass])
            {
                //"shielder", "saber", "archer", "lancer", "rider", "caster", "assassin", "berserker", "ruler", "avenger", "alter ego", "moon cancer", "foreigner"
                case "saber":
                    {
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "archer") result = 0.5;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "lancer") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "berserker") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "ruler") result = 0.5;
                        break;
                    }
                case "archer":
                    {
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "saber") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "lancer") result = 0.5;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "berserker") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "ruler") result = 0.5;
                        break;
                    }
                case "lancer":
                    {
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "saber") result = 0.5;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "archer") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "berserker") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "ruler") result = 0.5;
                        break;
                    }
                case "rider":
                    {
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "caster") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "assassin") result = 0.5;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "berserker") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "ruler") result = 0.5;
                        break;
                    }
                case "caster":
                    {
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "rider") result = 0.5;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "assassin") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "berserker") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "ruler") result = 0.5;
                        break;
                    }
                case "assassin":
                    {
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "rider") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "caster") result = 0.5;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "berserker") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "ruler") result = 0.5;
                        break;
                    }
                case "berserker":
                    {
                        if (formMain.className[enemyServant[attackedNo].servantClass] != "foreigner")
                        {
                            result = 1.5;
                        }
                        else
                        {
                            result = 0.5;
                        }
                        if (formMain.className[enemyServant[attackedNo].servantClass] != "shielder")
                        {
                            result = 1;
                        }
                        break;
                    }
                case "ruler":
                    {
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "avenger") result = 0.5;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "moon cancer") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "berserker") result = 2.0;
                        break;
                    }
                case "avenger":
                    {
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "ruler") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "moon cancer") result = 0.5;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "berserker") result = 2.0;
                        break;
                    }
                case "moon cancer":
                    {
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "ruler") result = 0.5;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "avenger") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "berserker") result = 2.0;
                        break;
                    }
                case "alter ego":
                    {
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "archer") result = 0.5;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "archer") result = 0.5;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "lancer") result = 0.5;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "rider") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "caster") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "assassin") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "berserker") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "foreigner") result = 2.0;
                        break;
                    }
                case "foreigner":
                    {
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "berserker") result = 2.0;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "alter ego") result = 0.5;
                        if (formMain.className[enemyServant[attackedNo].servantClass] == "foreigner") result = 2.0;
                        break;
                    }
                default:break;
            }
            return result;
        }
        public double calculateAttributeModifier(int attackerNo, int attackedNo)//可能
        {
            return 1.0;
        }

        //AI函数
        private int checkChain(List<Hit> checkList, int color)// 是否有color chain，返回有几个从者有该颜色卡
        {
            int countC = 0;
            int countS = 0;
            bool[] hasChain = new bool[checkList.Count];
            for (int i = 0; i < checkList.Count; i++)
            {
                hasChain[i] = false;
            }
            for (int i = 0; i < checkList.Count; i++)
            {
                if (checkList[i].hitType == color)
                {
                    if (color == 2)//蓝chain要看np
                    {
                        countC++;
                        if (hasChain[i] == false && !allyServant[checkList[i].attackerNo].NPavailable())
                        {
                            countS++;
                            hasChain[i] = true;
                        }
                    }
                    else//普通红绿chain直接
                    {
                        countC++;
                        if (hasChain[i] == false)
                        {
                            countS++;
                            hasChain[i] = true;
                        }
                    }
                }
            }
            if (countC >= chainNum && countS >= 1) 
            {
                return countS;
            }
            else
            {
                return 0;
            }
        }
        private int checkEX(List<Hit> checkList,List<int> NPlist)// 是否有3个相同从者卡，返回该从者NO
        {
            int[] countS = new int[3] { 0, 0, 0 };
            for (int i = 0; i < NPlist.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (j != NPlist[i])//非释放宝具人
                    {
                        countS[j] = -99;
                    }
                }
            }
            for (int i = 0; i < checkList.Count; i++)
            {
                countS[checkList[i].attackerNo]++;
            }
            for (int i = 0; i < servantNum; i++)
            {
                if (countS[i] >= chainNum)
                {
                    return i;
                }
            }
            return -1;
        }
        private bool checkCardType(List<Hit> checkList,int servantNo,int cardType,bool critNeed)//查看是否有某个从者的某张卡  0123白红蓝绿 4宝 critneed代表必须暴击
        {
            if (critNeed && nowStar < 20)//可能
            {
                return false;
            }
            for (int i = 0; i < checkList.Count; i++)
            {
                if (checkList[i].attackerNo == servantNo)
                {
                    if (cardType == 4 && checkList[i].isNP) return true;
                    if (cardType != 4 && !checkList[i].isNP && checkList[i].hitType == cardType) return true;
                }
            }
            return false;
        }
        private int maxPriority(int[] cardPriority, int type)//返回数组中最大的值的编号，并将其值清0
        {
            int max = 0;
            int No = -1;
            for (int i = 0; i < cardPriority.Length; i++)
            {
                if (cardPriority[i] >= max )
                {
                    if (type < 0) //没要求
                    {
                        max = cardPriority[i];
                        No = i;
                    }
                    else//有颜色要求
                    {
                        if (candidateHits[i].hitType == type)
                        {
                            max = cardPriority[i];
                            No = i;
                        }
                    }
                }
            }
            if (No != -1)//找到了目标
            {
                cardPriority[No] = 0;
            }
            return No;
        }
        public string showBuff(int No)//查看buff
        {
            string result = "效果名    数值 剩余回合\r\n";
            if (No != 3)
            {
                if (allyServant[No].buffs.Count == 0) return "无buff";
                for (int i = 0; i < allyServant[No].buffs.Count; i++)
                {
                    result += formMain.effectName[allyServant[No].buffs[i].type] + " ";
                    result += "     " + allyServant[No].buffs[i].value + " ";
                    result += "     " + ((allyServant[No].buffs[i].leftTurn==0)?"永久":allyServant[No].buffs[i].leftTurn.ToString()) + " ";
                    result += "\r\n";
                }
                result += "\r\n";
            }
            else
            {
                if (enemyServant[0].buffs.Count == 0) return "无debuff";
                for (int i = 0; i < enemyServant[0].buffs.Count; i++)
                {
                    result += formMain.effectName[enemyServant[0].buffs[i].type] + " ";
                    result += "     " + enemyServant[0].buffs[i].value + " ";
                    result += "     " + ((enemyServant[0].buffs[i].leftTurn == 0) ? "永久" : enemyServant[0].buffs[i].leftTurn.ToString()) + " ";
                    result += "\r\n";
                }
            }
            return result;
        }



    }
    

}
