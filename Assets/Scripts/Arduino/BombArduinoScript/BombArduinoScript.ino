
const int joyStickPin = 2;
#define VRX_PIN  A0 // Arduino pin connected to VRX pin
#define VRY_PIN  A1 // Arduino pin connected to VRY pin

int xValue = 0; // To store value of the X axis
int yValue = 0; // To store value of the Y axis

int minVal = 0;
int maxVal = 1023;
const int DEADZONE = 60; // adjust 20–100 depending on stick noise

//Button
const int buttonPin = 3;
int buttonState = 0;
int lastButtonState = buttonState;

void setup() {
  //Pins
  pinMode(joyStickPin, INPUT);
  pinMode(buttonPin, INPUT);
  
  //SerialPort
  Serial.begin(9600);
  Serial.flush();
}

void loop() {
  
  processJoyStick();
  //button input
  processButton();
  
}

void processJoyStick() {
    xValue = analogRead(VRX_PIN);
    yValue = analogRead(VRY_PIN);

    // 1. normalize to -1..1
    float x = (xValue - 512.0f) / 512.0f;
    float y = (yValue - 512.0f) / 512.0f;

    // 2. apply deadzone
    if (abs(x) < (DEADZONE / 512.0f)) x = 0;
    if (abs(y) < (DEADZONE / 512.0f)) y = 0;

    // 3. clamp magnitude (prevents 1,1 diagonal boost)
    float magnitude = sqrt(x * x + y * y);
    if (magnitude > 1.0f) {
        x /= magnitude;
        y /= magnitude;
    }

    // print result
    Serial.print("Joystick;");
    Serial.print(x);
    Serial.print(":");
    Serial.println(y);

    delay(200);
}

void processButton(){
  buttonState = digitalRead(buttonPin);
  if(buttonState != lastButtonState){
    lastButtonState = buttonState;
    if(buttonState == HIGH){
      Serial.println("Button;pressed");
    } else{
      Serial.println("Button;released");
    }
  }
}
