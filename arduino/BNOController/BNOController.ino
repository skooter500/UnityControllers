#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BNO055.h>
#include <utility/imumaths.h>
  
Adafruit_BNO055 bno = Adafruit_BNO055();
 
void setup() {
  Serial.begin(9600);  
  /* Initialise the sensor */
  if(!bno.begin())
  {
    /* There was a problem detecting the BNO055 ... check your connections */
    Serial.print("Ooops, no BNO055 detected ... Check your wiring or I2C ADDR!");
    while(1);
  }
  
  delay(1000);
    
  bno.setExtCrystalUse(true); 
}

void loop() {

  imu::Quaternion quat = bno.getQuat();
  /*if (quat.x() != lastQuat.x() 
    || quat.y() != lastQuat.y()
    || quat.z() != lastQuat.z()
    || quat.w() != lastQuat.w()
    )
    */
  {
    Serial.print("Q:");
    printDouble(quat.x(), 5);
    Serial.print(",");
    printDouble(quat.y(), 5);
    Serial.print(",");
    printDouble(quat.z(), 5);
    Serial.print(",");
    printDouble(quat.w(), 5);    
    Serial.println();

    uint8_t system, gyro, accel, mag;
    system = gyro = accel = mag = 0;
    bno.getCalibration(&system, &gyro, &accel, &mag);
    Serial.print("S:");
    Serial.println(system, DEC);
    Serial.print("G:");
    Serial.println(gyro, DEC);
    Serial.print("A:");
    Serial.println(accel, DEC);
    Serial.print("M:");
    Serial.println(mag, DEC);

    sensors_event_t event;
    bno.getEvent(&event);

    Serial.print(F("OX:"));
    Serial.println((float)event.orientation.x);
    Serial.print(F("OY:"));
    Serial.println((float)event.orientation.y);
    Serial.print(F("OZ:"));
    Serial.println((float)event.orientation.z);    
  }
  delay(100);  //lastQuat = quat;
}

void printDouble( double val, byte precision){
 // prints val with number of decimal places determine by precision
 // precision is a number from 0 to 6 indicating the desired decimial places
 // example: lcdPrintDouble( 3.1415, 2); // prints 3.14 (two decimal places)

 if(val < 0.0){
   Serial.print('-');
   val = -val;
 }

 Serial.print (int(val));  //prints the int part
 if( precision > 0) {
   Serial.print("."); // print the decimal point
   unsigned long frac;
   unsigned long mult = 1;
   byte padding = precision -1;
   while(precision--)
 mult *=10;

   if(val >= 0)
frac = (val - int(val)) * mult;
   else
frac = (int(val)- val ) * mult;
   unsigned long frac1 = frac;
   while( frac1 /= 10 )
padding--;
   while(  padding--)
Serial.print("0");
   Serial.print(frac,DEC) ;
 }
}

