#define _clear 0
#define pi 3.141592654
#define arm_length  3
/******************** declaration motors and it's pins ****************************/
#include <Servo.h>
#define ser_X_PIN 5
//#define ser_Y_PIN 6
#define ser_Z_PIN 7
Servo myservo_X,myservo_Y,myservo_Z;
/************************* declaration LCD and it's pins************************/ 
#include <LiquidCrystal.h>
#define RS  13
#define E   12
#define DB4 11
#define DB5 10
#define DB6 9
#define DB7 8
LiquidCrystal LCD (RS,E,DB4,DB5,DB6,DB7);
/************************* declaration KEYPAD and it's pins ************************/
#include <Keypad.h>
#define Row_1 A0
#define Row_2 A1
#define Row_3 A2
#define Row_4 A3
#define col_1 A4
#define col_2 A5
#define col_3 2
#define col_4 3
const byte ROWS = 4; //four rows
const byte COLS = 4; //four columns
char keys[ROWS][COLS] = {
  {'1','2','3','A'},
  {'4','5','6','B'},
  {'7','8','9','C'},
  {'*','0','#','D'}
};
byte rowPins[ROWS] = {Row_1,Row_2,Row_3,Row_4}; 
byte colPins[COLS] = {col_1,col_2,col_3,col_4}; 
Keypad k = Keypad( makeKeymap(keys), rowPins, colPins, ROWS, COLS );
/************************   declaration of wanted functions       *************************/
int charToint (char c);
int Reverse_value(int num,int iter );
void error ();
void _Name ();
void get_point ();
void F_text();
//void calculate_angles (float x_co_,float y_co_,float z_co_);
void printOnLCD (char text[20],int C_R,int C_COL,bool _remove ,float _time);
void angles(float val_x, float val_y, float val_z);
void lengths(float val_x, float val_y, float val_z);
void setup()
{
myservo_X.attach (ser_X_PIN);
//myservo_Y.attach (ser_Y_PIN);
myservo_Z.attach (ser_Z_PIN);
LCD.begin(16,2);
LCD.clear();
myservo_X.write(0);
myservo_Y.write(0);
myservo_Z.write(0);
F_text();
printOnLCD("Enter req_",0,0,_clear,2);
printOnLCD("Point ",1,5,!_clear,2);
}

void loop()
{  get_point ();
}
/*************** to find the integer value of input character from KEYPAD    *************************/
int charToint (char c)
{int value=c-'0';
   return value ;
}
/*************** to get the required point in 3D   **************************/
bool flag=0;
void get_point ()
{ while(1){
    if (flag)  {  printOnLCD("Enter next",0,0,_clear,2);
   printOnLCD("requi_ Point ",1,0,!_clear,2);}
  LCD.clear();
 LCD.setCursor(0,0);
 LCD.print("Point=(");
 int value_X=0;//get the reversed value X_cor
 int value_Y=0;//get the reversed value Y_cor
 int value_Z=0;//get the reversed value Z_cor
 int cycle_1=0;
 /********************/
 while(1){
 char input1 = k.waitForKey();//to Enter X coordinat
  if (input1=='A'&&cycle_1==0) error (); //if first iteration and there's no input make error and enter again
  else if (input1=='A') break;//to enter digits
  else if (input1=='B') _Name ();//show the names of members
  else if (input1=='C') get_point ();
  else LCD.print(input1);
  value_X+=charToint(input1)*pow(10,cycle_1);
 cycle_1++;   }
 int X_Cor=Reverse_value(value_X,cycle_1);
 LCD.print(",");
 /******************************/
 int cycle_2=0;
 while(1){
 char input2 = k.waitForKey();//to Enter Y coordinat
  if (input2=='A'&&cycle_2==0) error (); //if first iteration and there's no input make error and enter again
  else if (input2=='A') break;//to enter digits
  else if (input2=='B') _Name ();//show the names of members
  else if (input2=='C') get_point ();
  else LCD.print(input2);
  value_Y+=charToint(input2)*pow(10,cycle_2);
 cycle_2++;   }
 int Y_Cor=Reverse_value(value_Y,cycle_2);
 LCD.print(",");
 /************************/
 int cycle_3=0;
 while(1){
 char input3 = k.waitForKey();//to Enter Z coordinat
  if (input3=='A'&&cycle_3==0) error (); //if first iteration and there's no input make error and enter again
  else if (input3=='A') break;//to enter digits
  else if (input3=='B') _Name ();//show the names of members
  else if (input3=='C') get_point ();
  else LCD.print(input3);
  value_Z+=charToint(input3)*pow(10,cycle_3);
 cycle_3++;   }
 int Z_Cor=Reverse_value(value_Z,cycle_3);
 LCD.print(")");
 lengths(X_Cor,Y_Cor,Z_Cor);
 angles(X_Cor,Y_Cor,Z_Cor);
   flag=1;}
}
/************************* find the first formula of team **********************************/
void F_text()
{ printOnLCD("   welcome in  ",0,0,_clear,2);
  printOnLCD(" G.A.O.T   Team ",1,0,!_clear,3);
  printOnLCD("  Are u ready ?  ",0,0,_clear,3);
  } 
/***********************to get the required angles for motors***********************/ 
/**********Dot product=mag_vect1*mag_vect2*cos(angle between them)*****************/ 
    float XZlength = 0;
    float vlength = 0;
    void lengths(float val_x, float val_y, float val_z)
    {

        //calculate XZ projection of Target Vector
        XZlength = sqrt(pow(val_x, 2) + pow(val_z, 2));
        vlength = sqrt(pow(XZlength, 2) + pow(val_y, 2));
        if (XZlength> 2 * arm_length)
        {
            XZlength= 2 * arm_length;
        }

        //calculate Length of the Target Vector

        if(vlength>2 * arm_length )
        {
            vlength = 2 * arm_length;
        }
        }
void angles(float val_x, float val_y, float val_z)
{
        
        double angle_X = atan2(val_z, val_x)*(180/pi);
        double angle_XZ = atan2(XZlength , val_y) * (180/pi);
        double angle_phi = acos((pow(arm_length, 2) + pow(arm_length, 2)
                    - pow(vlength, 2)) / (2 * arm_length * arm_length)) * -(180/pi);
        double angle_theta = acos((pow(arm_length, 2) + pow(vlength, 2)
                    - pow(arm_length, 2)) / (2 * arm_length * vlength)) * (180/pi);

        double angle_theta_full = angle_theta + angle_XZ;    
        double angle_phi_full = 180.0 - angle_phi + angle_theta_full;
        for(int i=0;i<angle_X;i=i+5){myservo_X.write(i);delay(50);}
          myservo_X.write(angle_X);//to get max angle
          delay(50);
        for(int i=0;i<angle_phi_full;i=i+5){myservo_Z.write(i);delay(50);}
          myservo_Z.write(angle_phi_full);//to get max angle
          delay(50);
        for(int i=0;i<angle_theta_full;i=i+5){myservo_X.write(i);delay(50);}
          delay(50);     
}
/*void calculate_angles (float x_co_,float y_co_,float z_co_)
{ double AN_x_pro,AN_z_pro,Mag_Vect_,Mag_Vect_proj;
  Mag_Vect_=sqrt( pow(x_co_,2)+pow(y_co_,2)+pow(z_co_,2) );
  Mag_Vect_proj=sqrt( pow(x_co_,2)+pow(y_co_,2) );
  AN_x_pro=(x_co_/Mag_Vect_proj);//cos(angle),angle between x_axis and projection of vector in X_Y plane
  AN_x_pro=acos(AN_x_pro)*(180/pi); 
  AN_z_pro=( pow(Mag_Vect_proj,2)/(Mag_Vect_*Mag_Vect_proj) );//cos(angle),angle between projection of vector in X_Y plane and vector 
  AN_z_pro=acos(AN_z_pro)*(180/pi);
/**********************  next lines for moving shaft of motors ********************/
 /*for(int i=0;i<AN_x_pro;i=i+5){myservo_X.write(i);delay(50);}
 myservo_X.write(AN_x_pro);//to get max angle
 delay(50);
 for(int i=0;i<AN_z_pro;i=i+5){myservo_Z.write(i);delay(50);}
 myservo_Z.write(AN_z_pro);//to get max angle
 delay(50);
}*/
/*****to Print text on LCD with determinated delay time + clear LCD + cursor ************/
void printOnLCD (char text[20],int C_R,int C_COL,bool _remove ,float _time)
{ if (!_remove) LCD.clear();
  LCD.setCursor(C_COL,C_R);
  LCD.print(text);
  delay(_time*1000);
}
void _Name ()
{printOnLCD("names of ",0,0,_clear,2);
 printOnLCD("Team work ",1,0,!_clear,2);
 printOnLCD("Shehab    Aya",0,0,_clear,2);
 printOnLCD("Amira  Saleh",1,0,!_clear,2);
 printOnLCD("Assem  ",0,0,_clear,2);
 printOnLCD("Abdelrahman",1,0,!_clear,2);
 printOnLCD("Sherif  Saber",0,0,_clear,2);
 while(1){
 printOnLCD("GOOD BYE ",0,0,_clear,1);}
}
void error ()
{  LCD.clear();
  LCD.setCursor(0,0);
  LCD.print("  error  ");
  delay(3000);
  LCD.clear();
  get_point ();
}
/********************* to reverse the X/Y/Z coordinate to get the right value ****************************/
int Reverse_value(int num,int iter )
{int new_value=0;
  for (int i=iter-1;i>0;i--) 
 {  new_value+=(num%10)*pow(10,i);
    num/=10;        }
  return new_value;
}