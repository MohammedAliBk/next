"use client";
import { useEffect, useRef, useState } from "react";
import { Plus } from "lucide-react";

function AddTaskForm() {
  const [isOpen, setisOpen] = useState(false);
  const InputRef = useRef<HTMLInputElement>(null);
  useEffect(() => {
    if (isOpen) {
      console.log("open");
      InputRef.current?.focus();
    }
  }, [isOpen]);
  return (
    <main className="main-h-screen br-gray-100 relative">
      {/* add button */}
      <button
        onClick={() => setisOpen(true)}
        className="fixed bottom-10 right-10 w-14 h-14 rounded-full bg-purple-600 text-white flex items-center justify-center shadow-lg hover:scale-110 transition"
      >
        <Plus size={28}/>
      </button>
      {/* Input Model */}
      {isOpen && (
        <div className="fixed inset-0 bg-black/40 flex items-center justify-center">
          <div className="bg-white p-6 rounded-xl w-69 shadow-lg">
            <h2 className="text-lg font-bold mb-4">New Task</h2>
            <input
              ref={InputRef}
              className="w-full border p-2 rounded mb-4 "
              placeholder="Input your Task..."
            />
            <div className="flex justify-between gab-3">
              <button
                onClick={() => setisOpen(false)}
                className="px-4 py-2 rounded border"
              >
                Cancel
              </button>
              <button className="px-4 py-2 rounded bg-purple-600 text-white">
                Save
              </button>
            </div>
          </div>
        </div>
      )}
    </main>
  );
}

export default AddTaskForm;
