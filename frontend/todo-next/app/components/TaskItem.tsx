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
          <span className="font-semibold">{title}</span> 
        </div>
        {/* Edit task part */}
        <div className="flex gap-4 text-gray-400">
          <button>Edit</button>
          <button>Delete</button>
        </div>
      </li>
    </>
  );
}

export default TaskItem;
