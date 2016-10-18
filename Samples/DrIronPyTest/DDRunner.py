from DDNodeExecuter import *

#add reference to my lib
add_references_from_path('libs', ['DrAction.FSAction.dll'])

### Here we read tests DDNode from the files. For now it use static method from my lib.
test_ddNode = read_dd_node_from_file('testDDNode.xml')
test_non_static_node = read_dd_node_from_file('non_static_testDDnode.xml')
###

#static example
result = dd_run_static_method(test_ddNode) # resulting ddNode


#non static example
#fist case: invoking non static method of not existing object.
result, obj = dd_run_non_static_method(test_ddNode, constructor_params=[1]) #Returns resulting ddNode and instance
                                                                            # of object hat we create through invoking
                                                                            # constructor
print(type(obj))
print("this is first using of instance: {}".format(obj.non_static_field))

#second case: using non static method if already existing object.
result, obj = dd_run_non_static_method(test_ddNode, instance=obj)
print("this is second using of same instance: {}".format(obj.non_static_field))



