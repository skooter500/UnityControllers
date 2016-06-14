#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BNO055.h>
#include <utility/imumaths.h>
  
Adafruit_BNO055 bno = Adafruit_BNO055();

#define GPS 0x29 //GPS Shield I2C address

int PDOP = 0;
int HDOP = 0;
int VDOP = 0;
uint8_t statusReg  = 0;
long latitude   = 0;
long longitude  = 0;
long utcTime    = 0;
long date       = 0;
long altitude   = 0;
unsigned int course    = 0;
unsigned long speedKPH  = 0;
 
void setup() {
  Serial.begin(9600);  
  /* Initialise the sensor */
  if(!bno.begin(Adafruit_BNO055::OPERATION_MODE_COMPASS))
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

     Wire.beginTransmission(GPS);
    Wire.write((uint8_t)(0x00)); //set address pointer to zero
    Wire.endTransmission();
    delay(1); //1 millisecond delay is needed before requesting data
    Wire.requestFrom(GPS,1); //all data (not including checksum failures) is a total of 31 bytes
    statusReg = Wire.read();
    //lets check bit zero of the status register to see if any new data is available
    //if not we'll just return.  If bit zero is a 1 then new data is available
    //if(!bitRead(statusReg,0)){
    //  return;
    //}
    //let's check bit one of the status register to see if the latitude and
    //longitude data computed is valid or not.  This bit is controlled by the GPRMC sentence.
    //if the data is not valid we'll just return.  Invalid data is a good indicator that 
    //the GPS has not locked onto the satellites.
    if(!bitRead(statusReg,1)){
      Serial.println("No satellite signal lock");
      //delay(5000); //wait 5 seconds and check again
      //return;  //Un REM this line if you don't want to see all the invalid data
    }
    //Now let's collect the data. Since most data is larger than a single byte
    //we'll take advantage of loops to shift the data into the appropriate variables
    Wire.beginTransmission(GPS);
    Wire.write((uint8_t)(0x01)); //set address pointer to one
    Wire.endTransmission();
    delay(1); //1 millisecond delay is needed before requesting data
    Wire.requestFrom(GPS,32); 
    
    for(int i = 0;i < 4;i++){  //latitude is a Long so it is 4 bytes
      latitude <<= 8;
      latitude |= Wire.read();
    }
    for(int i = 0;i < 4;i++){
      longitude <<= 8;
      longitude |= Wire.read();
    }

    Serial.print("LAT:");
    printDouble((double) latitude / (double) 1000000, 5);
    Serial.println();
    Serial.print("LON:");
    printDouble((double) longitude / (double) 1000000, 5);
    Serial.println();
  }
  delay(10);  //lastQuat = quat;
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

