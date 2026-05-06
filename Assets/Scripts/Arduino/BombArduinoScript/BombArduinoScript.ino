
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

//Rotary Encoder
#include <ezButton.h>  // the library to use for SW pin

#define CLK_PIN 4
#define DT_PIN 5
#define SW_PIN 6

#define DIRECTION_CW 0   // clockwise direction
#define DIRECTION_CCW 1  // counter-clockwise direction

int counter = 0;
int minCounter = 0;
int maxCounter = 100;
int direction = DIRECTION_CW;
int CLK_state;
int prev_CLK_state;

ezButton button(SW_PIN);  // create ezButton object that attach to pin 4

//Keypad
#include <Keypad.h>

const int ROW_NUM = 4; //four rows
const int COLUMN_NUM = 4; //four columns

char keys[ROW_NUM][COLUMN_NUM] = {
  {'1','2','3', 'A'},
  {'4','5','6', 'B'},
  {'7','8','9', 'C'},
  {'*','0','#', 'D'}
};

byte pin_rows[ROW_NUM] = {30, 32, 34, 36}; //connect to the row pinouts of the keypad
byte pin_column[COLUMN_NUM] = {38, 40, 42, 44}; //connect to the column pinouts of the keypad

Keypad keypad = Keypad( makeKeymap(keys), pin_rows, pin_column, ROW_NUM, COLUMN_NUM );


void setup() {
  //Pins
  pinMode(joyStickPin, INPUT);
  pinMode(buttonPin, INPUT);
  // configure encoder pins as inputs
  //Rotary dingens
  pinMode(CLK_PIN, INPUT_PULLUP);
  pinMode(DT_PIN, INPUT_PULLUP);
  button.setDebounceTime(50);  // set debounce time to 50 milliseconds
  // read the initial state of the rotary encoder's CLK pin
  prev_CLK_state = digitalRead(CLK_PIN);


  
  //SerialPort
  Serial.begin(9600);
  Serial.flush();
}

void loop() {
  
  processJoyStick();
  //button input
  processButton();
  //rotary
  processRotaryDecoder();

  //keypad
  processKeyPad();
  
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
    if(magnitude > 0.1f){
      Serial.print("Joystick;");
      Serial.print(x);
      Serial.print(":");
      Serial.println(y);
      delay(200);
    }
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

void processRotaryDecoder(){
  button.loop();

  CLK_state = digitalRead(CLK_PIN);

  if (CLK_state != prev_CLK_state && CLK_state == HIGH) {

    if (digitalRead(DT_PIN) == HIGH) {
      counter--;
      if(counter < minCounter){
        counter = minCounter;
      }
      direction = DIRECTION_CCW;
    } else {
      counter++;
      if(counter > maxCounter){
        counter = maxCounter;
      }
      direction = DIRECTION_CW;
    }

    Serial.print("Potentiometer;");
    Serial.println(counter);
  }

  prev_CLK_state = CLK_state; // 🔥 REQUIRED
}

void processKeyPad(){
  char key = keypad.getKey();

  if (key){
    Serial.print("Numberpad;");
    Serial.println(key);
  }
  delay(10);
}