#define LEFT_POT 0
#define RIGHT_POT 1
#define SELECTOR 2
#define PRESSURE 2

int lastLeft = -1;
int lastRight = -1;
int lastSelector = -1;
int lastPressure = -1;

void setup()
{
  Serial.begin(9600);
  pinMode( PRESSURE, INPUT );
}

void loop() {
  int left = constrain(analogRead(LEFT_POT) / 90, 0, 10);
  int right = constrain(analogRead(RIGHT_POT) / 90, 0, 10);
  int selector = digitalRead(SELECTOR);
  int pressure = analogRead(PRESSURE);

  if (left != lastLeft)  
  {
    Serial.print("L:");
    Serial.println(left);
  }

  if (right != lastRight)  
  {
    Serial.print("R:");
    Serial.println(right);
  }

  if (selector != lastSelector)  
  {
    Serial.print("S:");
    Serial.println(selector);
  }
/*
  if (pressure != lastPressure)  
  {
    Serial.print("P:");
    Serial.println(pressure);
  }
  */
  lastLeft = left;  
  lastRight = right;  
  lastSelector = selector;
  lastPressure = pressure;  
}
