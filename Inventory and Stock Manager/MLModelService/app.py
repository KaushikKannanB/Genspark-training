from flask import Flask, request, jsonify
import joblib

app = Flask(__name__)

try:
    model = joblib.load("model.pkl")
except Exception as e:
    print(f"Error loading model: {e}")
    model = None

@app.route("/predict", methods=["POST"])
def predict():
    if not model:
        return jsonify({"error": "Model not loaded."}), 500

    try:
        data = request.get_json()
        question = data.get("question", "")
        prediction = model.predict([question])[0]
        return jsonify({"intent": prediction})
    except Exception as e:
        return jsonify({"error": str(e)}), 500

if __name__ == "__main__":
    app.run(debug=True)