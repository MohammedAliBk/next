import { Pencil, Trash2 } from "lucide-react";

type TaskItemProps = {
  title: string;
};

export function TaskItem({ title }: TaskItemProps) {
  return (
    <>
      <li className="flex items-center justify-between py-4">
        {/* Task text */}
        <div className="flex items-center gap-3">
          <input type="checkbox" className="w-5 h-5 accent-purple-500" />
          <span className="font-semibold cursor-pointer">{title}</span>
        </div>
        {/* Edit task part */}
        <div className="flex gap-4 text-gray-400">
          <button className=" hover:text-purple-500 hover:scale-110 transition cursor-pointer">
            <Pencil />
          </button>
          <button className=" hover:text-red-500 hover:scale-110 transition cursor-pointer">
            <Trash2 />
          </button>
        </div>
      </li>
    </>
  );
}

export default TaskItem;
